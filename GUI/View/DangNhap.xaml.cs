using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using BUS;
using MySql.Data.MySqlClient;

namespace GUI.View
{
	public partial class DangNhap : Window
	{
		private bool _isPasswordVisible;
		public bool IsPasswordVisible
		{
			get => _isPasswordVisible;
			set
			{
				_isPasswordVisible = value;
				UpdatePasswordVisibility();
			}
		}

		public DangNhap()
		{
			InitializeComponent();
			MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
			// Khởi tạo trạng thái checkbox
			if (Properties.Settings.Default.RememberMe)
			{
				ckb_GhiNho.IsChecked = true;
				txtUsername.Text = Properties.Settings.Default.AcUsername;
				txtPasswordHidden.Password = Properties.Settings.Default.AcPassword;
				txtPasswordVisible.Text = txtPasswordHidden.Password;
			}
			else
			{
				ckb_GhiNho.IsChecked = false;
			}
		}

		private void btn_Close_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default.Save();
			var thongbao = new DialogCustoms("Bạn có thật sự muốn thoát!", "Thông báo", DialogCustoms.YesNo);
			if (thongbao.ShowDialog() == true) Close();
		}

		private bool KiemTraKetNoiDatabase()
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var connection = new MySqlConnection(connectionString))
				{
					connection.Open(); // Mở kết nối
					connection.Close(); // Đóng kết nối nếu thành công
					return true; // Kết nối thành công
				}
			}
			catch
			{
				return false; // Không thể kết nối
			}
		}

		private void click_DangNhap(object sender, RoutedEventArgs e)
		{
			if (!KiemTraKetNoiDatabase())
			{
				new DialogCustoms(
					"Không thể kết nối tới cơ sở dữ liệu.\nVui lòng kiểm tra kết nối mạng hoặc cấu hình máy chủ!",
					"Thông báo", DialogCustoms.OK).ShowDialog();
				return; // Dừng xử lý nếu không kết nối được
			}

			var username = txtUsername.Text.Trim();
			if (string.IsNullOrWhiteSpace(username))
			{
				new DialogCustoms("Vui lòng nhập tên đăng nhập!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			var pass = txtPasswordHidden.Password;
			var taiKhoan = TaiKhoanNVBUS.GetInstance().kiemTraTKTonTaiKhong(username, pass);

			if (taiKhoan != null)
			{
				if (taiKhoan.IsDeleted)
				{
					new DialogCustoms("Tài khoản không tồn tại!", "Thông báo", DialogCustoms.OK).ShowDialog();
					return;
				}

				if (taiKhoan.Disabled)
				{
					new DialogCustoms("Tài khoản đã bị vô hiệu hóa!", "Thông báo", DialogCustoms.OK).ShowDialog();
					return;
				}

				new DialogCustoms("Đăng nhập thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
				var main = new MainWindow(taiKhoan);
				main.Show();
				Close();
			}
			else
			{
				new DialogCustoms("Không tồn tại tài khoản hoặc mật khẩu không đúng!", "Thông báo", DialogCustoms.OK)
					.ShowDialog();
			}
		}

		private void txbMatKhau_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter) // Kiểm tra nếu phím Enter được nhấn
				click_DangNhap(sender, e); // Gọi hàm xử lý khi click nút đăng nhập
		}

		private void Hyperlink_Click(object sender, RoutedEventArgs e)
		{
			DangNhap_QuenMK quenMK = new DangNhap_QuenMK();
			quenMK.ShowDialog();
		}

		private void Hide_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			IsPasswordVisible = !IsPasswordVisible;
		}

		private void txtPasswordHidden_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (!IsPasswordVisible)
			{
				txtPasswordVisible.Text = txtPasswordHidden.Password;
			}
		}

		private void UpdatePasswordVisibility()
		{
			if (IsPasswordVisible)
			{
				txtPasswordVisible.Text = txtPasswordHidden.Password;
				txtPasswordHidden.Visibility = Visibility.Collapsed;
				txtPasswordVisible.Visibility = Visibility.Visible;
				iconEye.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;
			}
			else
			{
				txtPasswordHidden.Password = txtPasswordVisible.Text;
				txtPasswordVisible.Visibility = Visibility.Collapsed;
				txtPasswordHidden.Visibility = Visibility.Visible;
				iconEye.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;
			}
		}

		private void ckb_GhiNho_Checked(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPasswordHidden.Password))
			{
				if (ckb_GhiNho.IsChecked == true)
				{
					Properties.Settings.Default.RememberMe = true;
					Properties.Settings.Default.AcUsername = txtUsername.Text;
					Properties.Settings.Default.AcPassword = txtPasswordHidden.Password;
				}
				else
				{
					Properties.Settings.Default.RememberMe = false;
					Properties.Settings.Default.AcUsername = string.Empty;
					Properties.Settings.Default.AcPassword = string.Empty;
				}
				Properties.Settings.Default.Save();
			}
		}
	}
}
