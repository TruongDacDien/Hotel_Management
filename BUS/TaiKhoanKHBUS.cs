using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class TaiKhoanKHBUS
	{
		private static TaiKhoanKHBUS Instance;

		private readonly CloudinaryService _cloudinaryService = new CloudinaryService();

		private TaiKhoanKHBUS()
		{
		}

		public static TaiKhoanKHBUS GetInstance()
		{
			if (Instance == null) Instance = new TaiKhoanKHBUS();
			return Instance;
		}

		public TaiKhoanKH kiemTraTKTonTaiKhong(string username, string pass)
		{
			// Lấy tài khoản từ cơ sở dữ liệu theo username
			TaiKhoanKH taiKhoan = TaiKhoanKHDAL.GetInstance().layTaiKhoanTheoUsername(username);

			if (taiKhoan == null)
				// Không tồn tại tài khoản
				return null;

			// Kiểm tra mật khẩu nhập vào có khớp với hash trong cơ sở dữ liệu
			var isPasswordMatch = Bcrypt_HashBUS.GetInstance().VerifyMatKhau(pass, taiKhoan.Password);
			return isPasswordMatch ? taiKhoan : null;
		}

		public async Task<string> UploadAvatarAsync(string filePath, string publicId, string oldAvatarId)
		{
			if (publicId != oldAvatarId && publicId != "hotel_management/user_default")
			{
				// Xoá ảnh cũ nếu có up ảnh mới
				await _cloudinaryService.DeleteImageAsync(publicId);
			}

			// Upload ảnh mới
			var url = await _cloudinaryService.UploadImageAsync(filePath, publicId);
			return url;
		}

		public bool capNhatAvatar(int maTKKH, string avatarId, string avatarURL)
		{
			return TaiKhoanKHDAL.GetInstance().capNhatAvatarKH(maTKKH, avatarId, avatarURL);
		}

		public List<TaiKhoanKH> getDataTaiKhoan()
		{
			return TaiKhoanKHDAL.GetInstance().getDataTaiKhoanKH();
		}

		public TaiKhoanKH layTaiKhoanTheoUsername(string username)
		{
			return TaiKhoanKHDAL.GetInstance().layTaiKhoanTheoUsername(username);
		}

		public bool kiemTraTrungUsername(string username)
		{
			return TaiKhoanKHDAL.GetInstance().kiemTraTrungUsernameKH(username);
		}

		public bool kiemTraTrungEmail(string email)
		{
			return TaiKhoanKHDAL.GetInstance().kiemTraTrungEmailKH(email);
		}

		public bool xoaTaiKhoan(TaiKhoanKH taiKhoan)
		{
			return TaiKhoanKHDAL.GetInstance().xoaTaiKhoanKH(taiKhoan);
		}

		public bool themTaiKhoan(TaiKhoanKH taiKhoan)
		{
			return TaiKhoanKHDAL.GetInstance().themTaiKhoanKH(taiKhoan);
		}

		public bool capNhatTaiKhoan(TaiKhoanKH taiKhoan)
		{
			return TaiKhoanKHDAL.GetInstance().capNhatTaiKhoanKH(taiKhoan);
		}

		public bool hienThiLaiTaiKhoan(string username)
		{
			return TaiKhoanKHDAL.GetInstance().hienThiLaiTaiKhoanKH(username);
		}
	}
}