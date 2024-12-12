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
			// Lấy tài khoản từ cơ sở dữ liệu theo username
			TaiKhoan taiKhoan = TaiKhoanDAL.GetInstance().layTaiKhoanTheoUsername(username);
           
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

        public List<TaiKhoan> getDataTaiKhoan()
        {
            return TaiKhoanDAL.GetInstance().getDataTaiKhoan();
        }

        public TaiKhoan layTaiKhoanTheoUsername(string username)
        {
            return TaiKhoanDAL.GetInstance().layTaiKhoanTheoUsername(username);
        }

        public bool kiemTraTrungUsername(string username)
        {
            return TaiKhoanDAL.GetInstance().kiemTraTrungUsername(username);
        }

        public bool xoaTaiKhoan(TaiKhoan taiKhoan)
        {
            return TaiKhoanDAL.GetInstance().xoaTaiKhoan(taiKhoan);
        }

        public bool themTaiKhoan(TaiKhoan taiKhoan)
        {
            return TaiKhoanDAL.GetInstance().themTaiKhoan(taiKhoan);
        }

		public bool capNhatTaiKhoan(TaiKhoan taiKhoan)
		{
			return TaiKhoanDAL.GetInstance().capNhatTaiKhoan(taiKhoan);
		}

        public bool hienThiLaiTaiKhoan(string username)
        {
            return TaiKhoanDAL.GetInstance().hienThiLaiTaiKhoan(username);
        }
	}
}
