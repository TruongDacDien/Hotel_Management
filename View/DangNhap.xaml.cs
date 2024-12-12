using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using BUS;
using DAL.Data;
using DAL.DTO;
using System.Configuration;

namespace GUI.View
{
	/// <summary>
	/// Interaction logic for DangNhap.xaml
	/// </summary>
	public partial class DangNhap : Window
	{
		public DangNhap()
		{
			InitializeComponent();
			this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = false;
		}


		private void btn_Close_Click(object sender, RoutedEventArgs e)
		{
			var thongbao = new DialogCustoms("Bạn có thật sự muốn thoát!", "Thông báo", DialogCustoms.YesNo);
			if (thongbao.ShowDialog() == true)
			{
				this.Close();
			}
		}

		private bool KiemTraKetNoiDatabase()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection connection = new MySqlConnection(connectionString))
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
				new DialogCustoms("Không thể kết nối tới cơ sở dữ liệu.\nVui lòng kiểm tra kết nối mạng hoặc cấu hình máy chủ!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return; // Dừng xử lý nếu không kết nối được
			}
			string username = txbTenDangNhap.Text.Trim();
			string pass = txbMatKhau.Password;
			TaiKhoan taiKhoan = TaiKhoanBUS.GetInstance().kiemTraTKTonTaiKhong(username, pass);
			
			if (taiKhoan != null)
			{
				if(taiKhoan.Disabled == true)
				{
					new DialogCustoms("Tài khoản đã bị vô hiệu hóa!", "Thông báo", DialogCustoms.OK).ShowDialog();
					return;
				}
				new DialogCustoms("Đăng nhập thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
				MainWindow main = new MainWindow(taiKhoan);
				main.Show();
				this.Close();
			}
			else
			{
				new DialogCustoms("Không tồn tại tài khoản hoặc mật khẩu không đúng!", "Thông báo", DialogCustoms.OK).ShowDialog();
			}
		}
        private void txbMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // Kiểm tra nếu phím Enter được nhấn
            {
                click_DangNhap(sender, e); // Gọi hàm xử lý khi click nút đăng nhập
            }
        }
    }
}
