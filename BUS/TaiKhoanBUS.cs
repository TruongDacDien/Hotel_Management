using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Data;
using System.Windows;

namespace BUS
{
    public class TaiKhoanBUS
    {
        private static TaiKhoanBUS Instance;

        private TaiKhoanBUS()
        {

        }

        public static TaiKhoanBUS GetInstance()
        {
            if (Instance == null)
            {
                Instance = new TaiKhoanBUS();
            }
            return Instance;
        }
		public TaiKhoan kiemTraTKTonTaiKhong(string username, string pass)
		{
			MessageBox.Show("Dữ liệu đã được tải thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
			// Lấy tài khoản từ cơ sở dữ liệu theo username
			TaiKhoan taiKhoan = TaiKhoanDAL.GetInstance().layTaiKhoanTheoUsername(username);
			MessageBox.Show("Tài Khoản: " +taiKhoan.Username.ToString(), "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
			if (taiKhoan == null)
			{
				// Không tồn tại tài khoản
				return null;
			}

			// Kiểm tra mật khẩu nhập vào có khớp với hash trong cơ sở dữ liệu
			bool isPasswordMatch = Bcrypt_HashBUS.GetInstance().VerifyMatKhau(pass, taiKhoan.Password);

			return isPasswordMatch ? taiKhoan : null;
		}

		public bool capNhatAvatar(string username,byte[] avatar, out string error)
        {
            return TaiKhoanDAL.GetInstance().capNhatAvatar( username , avatar, out error);
        }
    }
}
