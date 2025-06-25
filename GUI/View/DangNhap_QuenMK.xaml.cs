using BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using DocumentFormat.OpenXml.EMMA;
using DAL.DTO;

namespace GUI.View
{
	/// <summary>
	/// Interaction logic for DangNhap_QuenMK.xaml
	/// </summary>
	public partial class DangNhap_QuenMK : Window
	{
		public DangNhap_QuenMK()
		{
			InitializeComponent();
		}

		private void btn_Close_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private async void btn_XacNhan_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtCCCD.Text))
			{
				new DialogCustoms("Nhập đầy đủ thông tin để xác nhận!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}	
			if (!TaiKhoanNVBUS.GetInstance().kiemTraTrungUsername(txtUsername.Text))
			{
				new DialogCustoms("Không tìm thấy tài khoản!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}
			if (!TaiKhoanNVBUS.GetInstance().kiemTraTrungEmail(txtEmail.Text))
			{
				new DialogCustoms("Email chưa được đăng ký!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}
			if (NhanVienBUS.GetInstance().kiemTraTonTaiNhanVien(txtCCCD.Text) == -1)
			{
				new DialogCustoms("Không tồn tại nhân viên với căn cước đã nhập!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			// Lấy thông tin tài khoản
			TaiKhoanNV taiKhoan = TaiKhoanNVBUS.GetInstance().layTaiKhoanTheoUsername(txtUsername.Text.Trim());
			if (taiKhoan == null)
			{
				new DialogCustoms("Không lấy được thông tin tài khoản!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}
			if (taiKhoan.NhanVien.IsDeleted == true)
			{
				new DialogCustoms("Không tồn tại nhân viên tương ứng với tài khoản!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}
			if (taiKhoan.Disabled == true)
			{
				new DialogCustoms("Tài khoản đã bị khóa vui lòng liên hệ với quản trị viên!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			// Tạo mật khẩu mới
			string newPassword = GenerateRandomPassword();
			taiKhoan.Password = Bcrypt_HashBUS.GetInstance().HashMatKhau(newPassword);

			// Cập nhật mật khẩu mới vào cơ sở dữ liệu
			bool isUpdated = TaiKhoanNVBUS.GetInstance().capNhatTaiKhoan(taiKhoan);
			if (!isUpdated)
			{
				new DialogCustoms("Không thể cập nhật mật khẩu. Vui lòng thử lại sau!", "Lỗi", DialogCustoms.OK).ShowDialog();
				return;
			}

			// Thông tin email
			string fromMail = Properties.Resources.Email;
			string fromPassword = Properties.Resources.Password;
			string displayName = Properties.Resources.DisplayName;
			string host = Properties.Resources.Host;
			int port = Int32.Parse(Properties.Resources.Port);

			var fromAddress = new MailAddress(fromMail, displayName);
			var toAddress = new MailAddress(txtEmail.Text);

			var smtp = new SmtpClient(host, port)
			{
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(fromMail, fromPassword),
				Timeout = 20000
			};

			// Nội dung email
			var message = new MailMessage(fromAddress, toAddress)
			{
				Subject = "Reset Your Password",
				Body = $"Dear {taiKhoan.NhanVien.HoTen},\n\n" +
					   $"Your new password is: {newPassword}\n\n" +
					   "Please log in with this password and change it immediately for security purposes.\n\n" +
					   "If you did not request this, please contact our support team.\n\n" +
					   "Best regards,\nThe Support Team",
				IsBodyHtml = false // Bạn có thể đặt là true nếu muốn định dạng HTML
			};

			try
			{
				// Gửi email
				await smtp.SendMailAsync(message);
				new DialogCustoms("Mật khẩu mới đã được gửi qua email.", "Thông báo", DialogCustoms.OK).ShowDialog();
			}
			catch (Exception ex)
			{
				new DialogCustoms($"Không thể gửi email: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();	
			}
			this.Close();
		}

		// Hàm tạo mật khẩu ngẫu nhiên
		private string GenerateRandomPassword()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();
			return new string(Enumerable.Repeat(chars, 6) // Độ dài mật khẩu: 6 ký tự
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
