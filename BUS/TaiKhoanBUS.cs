using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class TaiKhoanBUS
	{
		private static TaiKhoanBUS Instance;

		private readonly CloudinaryService _cloudinaryService = new CloudinaryService();

		private TaiKhoanBUS()
		{
		}

		public static TaiKhoanBUS GetInstance()
		{
			if (Instance == null) Instance = new TaiKhoanBUS();
			return Instance;
		}

		public TaiKhoanNV kiemTraTKTonTaiKhong(string username, string pass)
		{
			// Lấy tài khoản từ cơ sở dữ liệu theo username
			TaiKhoanNV taiKhoan = TaiKhoanDAL.GetInstance().layTaiKhoanTheoUsername(username);

			if (taiKhoan == null)
				// Không tồn tại tài khoản
				return null;

			// Kiểm tra mật khẩu nhập vào có khớp với hash trong cơ sở dữ liệu
			var isPasswordMatch = Bcrypt_HashBUS.GetInstance().VerifyMatKhau(pass, taiKhoan.Password);
			return isPasswordMatch ? taiKhoan : null;
		}

		public async Task<string> UploadAvatarAsync(string filePath, string publicId, string oldAvatarId)
		{
			if(publicId != oldAvatarId && publicId != "hotel_management/user_default_m4o3wc")
			{
				// Xoá ảnh cũ nếu có up ảnh mới
				await _cloudinaryService.DeleteImageAsync(publicId);
			}

			// Upload ảnh mới
			var url = await _cloudinaryService.UploadImageAsync(filePath, publicId);
			return url;
		}

		public bool capNhatAvatar(string username, string avatarId, string avatarURL, out string error)
		{
			return TaiKhoanDAL.GetInstance().capNhatAvatar(username, avatarId, avatarURL, out error);
		}

		public List<TaiKhoanNV> getDataTaiKhoan()
		{
			return TaiKhoanDAL.GetInstance().getDataTaiKhoan();
		}

		public TaiKhoanNV layTaiKhoanTheoUsername(string username)
		{
			return TaiKhoanDAL.GetInstance().layTaiKhoanTheoUsername(username);
		}

		public bool kiemTraTrungUsername(string username)
		{
			return TaiKhoanDAL.GetInstance().kiemTraTrungUsername(username);
		}

		public bool kiemTraTrungEmail(string email)
		{
			return TaiKhoanDAL.GetInstance().kiemTraTrungEmail(email);
		}

		public bool xoaTaiKhoan(TaiKhoanNV taiKhoan)
		{
			return TaiKhoanDAL.GetInstance().xoaTaiKhoan(taiKhoan);
		}

		public bool themTaiKhoan(TaiKhoanNV taiKhoan)
		{
			return TaiKhoanDAL.GetInstance().themTaiKhoan(taiKhoan);
		}

		public bool capNhatTaiKhoan(TaiKhoanNV taiKhoan)
		{
			return TaiKhoanDAL.GetInstance().capNhatTaiKhoan(taiKhoan);
		}

		public bool hienThiLaiTaiKhoan(string username)
		{
			return TaiKhoanDAL.GetInstance().hienThiLaiTaiKhoan(username);
		}

		public PhanQuyen layPhanQuyenTaiKhoan(int maTKNV)
		{
			return TaiKhoanDAL.GetInstance().layPhanQuyenTaiKhoan(maTKNV);
		}
	}
}