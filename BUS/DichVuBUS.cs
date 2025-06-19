using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class DichVuBUS
	{
		private static DichVuBUS Instance;

		private readonly CloudinaryService _cloudinaryService = new CloudinaryService();

		private DichVuBUS()
		{
		}

		public static DichVuBUS GetInstance()
		{
			if (Instance == null) Instance = new DichVuBUS();
			return Instance;
		}

		public List<DichVu> getDichVu_Custom()
		{
			return DichVuDAL.GetInstance().getDataDichVu_Custom();
		}

		public List<DichVu> getDichVu()
		{
			return DichVuDAL.GetInstance().getDataDichVu();
		}

		public bool ThemDichVu(DichVu dv)
		{
			return DichVuDAL.GetInstance().addDichVu(dv);
		}

		public void xoaDataDichVu(DichVu dv)
		{
			DichVuDAL.GetInstance().xoaDichVu(dv);
		}

		public bool capNhatDichVu(DichVu dv)
		{
			return DichVuDAL.GetInstance().capNhatDichVu(dv);
		}

		public bool KiemTraTrungTen(DichVu dv)
		{
			return DichVuDAL.GetInstance().KiemTraTrungTen(dv);
		}

		public bool hienThiLaiDichVu(string tenDV)
		{
			return DichVuDAL.GetInstance().hienThiLaiDichVu(tenDV);
		}

		public async Task<string> UploadImageAsync(string filePath, string publicId, string oldImageId)
		{
			if (publicId != oldImageId && publicId != "hotel_management/image_default")
			{
				// Xoá ảnh cũ nếu có up ảnh mới
				await _cloudinaryService.DeleteImageAsync(publicId);
			}

			// Upload ảnh mới
			var url = await _cloudinaryService.UploadImageAsync(filePath, publicId);
			return url;
		}
	}
}