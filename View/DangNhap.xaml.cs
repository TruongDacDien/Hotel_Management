using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using BUS;
using MySql.Data.MySqlClient;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for DangNhap.xaml
	/// </summary>
	public partial class DangNhap : Window
	{
		public DangNhap()
		{
			InitializeComponent();
			MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = false;
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

			var username = txbTenDangNhap.Text.Trim();
			var pass = txbMatKhau.Password;
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
	}
}