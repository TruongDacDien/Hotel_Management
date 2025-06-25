using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class TaiKhoanNVBUS
	{
		private static TaiKhoanNVBUS Instance;

		private readonly CloudinaryService _cloudinaryService = new CloudinaryService();

		private TaiKhoanNVBUS()
		{
		}

		public static TaiKhoanNVBUS GetInstance()
		{
			if (Instance == null) Instance = new TaiKhoanNVBUS();
			return Instance;
		}

		public TaiKhoanNV kiemTraTKTonTaiKhong(string username, string pass)
		{
			// Lấy tài khoản từ cơ sở dữ liệu theo username
			TaiKhoanNV taiKhoan = TaiKhoanNVDAL.GetInstance().layTaiKhoanTheoUsername(username);

			if (taiKhoan == null)
				// Không tồn tại tài khoản
				return null;

			// Kiểm tra mật khẩu nhập vào có khớp với hash trong cơ sở dữ liệu
			var isPasswordMatch = Bcrypt_HashBUS.GetInstance().VerifyMatKhau(pass, taiKhoan.Password);
			return isPasswordMatch ? taiKhoan : null;
		}

		public async Task<string> UploadAvatarAsync(string filePath, string publicId, string oldAvatarId)
		{
			if(publicId != oldAvatarId && publicId != "hotel_management/user_default")
			{
				// Xoá ảnh cũ nếu có up ảnh mới
				await _cloudinaryService.DeleteImageAsync(publicId);
			}

			// Upload ảnh mới
			var url = await _cloudinaryService.UploadImageAsync(filePath, publicId);
			return url;
		}

		public bool capNhatAvatar(int maTKNV, string avatarId, string avatarURL)
		{
			return TaiKhoanNVDAL.GetInstance().capNhatAvatar(maTKNV, avatarId, avatarURL);
		}

		public List<TaiKhoanNV> getDataTaiKhoan()
		{
			return TaiKhoanNVDAL.GetInstance().getDataTaiKhoan();
		}

		public TaiKhoanNV layTaiKhoanTheoUsername(string username)
		{
			return TaiKhoanNVDAL.GetInstance().layTaiKhoanTheoUsername(username);
		}

		public bool kiemTraTrungUsername(string username)
		{
			return TaiKhoanNVDAL.GetInstance().kiemTraTrungUsername(username);
		}

		public bool kiemTraTrungEmail(string email)
		{
			return TaiKhoanNVDAL.GetInstance().kiemTraTrungEmail(email);
		}

		public bool xoaTaiKhoan(TaiKhoanNV taiKhoan)
		{
			return TaiKhoanNVDAL.GetInstance().xoaTaiKhoan(taiKhoan);
		}

		public bool themTaiKhoan(TaiKhoanNV taiKhoan)
		{
			return TaiKhoanNVDAL.GetInstance().themTaiKhoan(taiKhoan);
		}

		public bool capNhatTaiKhoan(TaiKhoanNV taiKhoan)
		{
			return TaiKhoanNVDAL.GetInstance().capNhatTaiKhoan(taiKhoan);
		}

		public bool hienThiLaiTaiKhoan(string username)
		{
			return TaiKhoanNVDAL.GetInstance().hienThiLaiTaiKhoan(username);
		}

		public PhanQuyen layPhanQuyenTaiKhoan(int maTKNV)
		{
			return TaiKhoanNVDAL.GetInstance().layPhanQuyenTaiKhoan(maTKNV);
		}

		public bool capNhatPhanQuyenTaiKhoan(PhanQuyen pq)
		{
			return TaiKhoanNVDAL.GetInstance().capNhatPhanQuyenTaiKhoan(pq);
		}

		public bool themPhanQuyenTaiKhoan(PhanQuyen pq)
		{
			return TaiKhoanNVDAL.GetInstance().themPhanQuyenTaiKhoan(pq);
		}

		public bool xoaPhanQuyenTaiKhoan(PhanQuyen pq)
		{
			return TaiKhoanNVDAL.GetInstance().xoaPhanQuyenTaiKhoan(pq);
		}
	}
}