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
		}

		private void btn_Close_Click(object sender, RoutedEventArgs e)
		{
			var thongbao = new DialogCustoms("Bạn có thật sự muốn thoát!", "Thông báo", DialogCustoms.YesNo);
			if (thongbao.ShowDialog() == true) Close();
		}

		private bool KiemTraKetNoiDatabase()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

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
			var taiKhoan = TaiKhoanBUS.GetInstance().kiemTraTKTonTaiKhong(username, pass);

			if (taiKhoan != null)
			{
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
			string url = "https://www.facebook.com/truong.ac.ien";
			var thongbao = new DialogCustoms("Liên hệ admin để lấy lại mật khẩu!", "Thông báo", DialogCustoms.OK);
			thongbao.ShowDialog();
			try
			{
				Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
			}
			catch (Exception ex)
			{
				var thongbaoloi = new DialogCustoms($"Không thể mở liên kết: {ex.Message}", "Lỗi", DialogCustoms.OK);
				thongbaoloi.ShowDialog();
			}
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
	}
}
