using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class LoaiPhongBUS
	{
		private static LoaiPhongBUS instance;

		private readonly CloudinaryService _cloudinaryService = new CloudinaryService();

		private LoaiPhongBUS()
		{
		}

		public static LoaiPhongBUS GetInstance()
		{
			if (instance == null) instance = new LoaiPhongBUS();
			return instance;
		}

		public List<LoaiPhong> getDataLoaiPhong()
		{
			return LoaiPhongDAL.GetInstance().getDataLoaiPhong();
		}

		public LoaiPhong getLoaiPhongTheoMaLoaiPhong(int maLoaiPhong)
		{
			return LoaiPhongDAL.GetInstance().getLoaiPhongTheoMaLoaiPhong(maLoaiPhong);
		}

		public bool addLoaiPhong(LoaiPhong loaiPhong)
		{
			return LoaiPhongDAL.GetInstance().addLoaiPhong(loaiPhong);
		}

		public bool xoaLoaiPhong(LoaiPhong loaiPhong)
		{
			return LoaiPhongDAL.GetInstance().xoaLoaiPhong(loaiPhong);
		}

		public bool capNhatDataLoaiPhong(LoaiPhong loaiPhong)
		{
			return LoaiPhongDAL.GetInstance().capnhatLoaiPhong(loaiPhong);
		}

		public bool KiemTraTrungTen(LoaiPhong loaiPhong)
		{
			return LoaiPhongDAL.GetInstance().KiemTraTenLoaiPhong(loaiPhong);
		}

		public bool hienThiLaiLoaiPhong(string tenLoaiPhong)
		{
			return LoaiPhongDAL.GetInstance().hienThiLaiLoaiPhong(tenLoaiPhong);
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