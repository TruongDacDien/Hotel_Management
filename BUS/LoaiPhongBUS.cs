using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class LoaiPhongBUS
	{
		private static LoaiPhongBUS instance;

		private LoaiPhongBUS()
		{
		}

		public static LoaiPhongBUS Instance
		{
			get
			{
				if (instance == null) instance = new LoaiPhongBUS();
				return instance;
			}
		}

		public List<LoaiPhong> getDataLoaiPhong()
		{
			return LoaiPhongDAL.Instance.getDataLoaiPhong();
		}

		public bool addLoaiPhong(LoaiPhong loaiPhong)
		{
			return LoaiPhongDAL.Instance.addLoaiPhong(loaiPhong);
		}

		public bool xoaLoaiPhong(LoaiPhong loaiPhong)
		{
			return LoaiPhongDAL.Instance.xoaLoaiPhong(loaiPhong);
		}

		public bool capNhatDataLoaiPhong(LoaiPhong loaiPhong)
		{
			return LoaiPhongDAL.Instance.capnhatLoaiPhong(loaiPhong);
		}

		public bool KiemTraTrungTen(LoaiPhong loaiPhong)
		{
			return LoaiPhongDAL.Instance.KiemTraTenLoaiPhong(loaiPhong);
		}

		public bool hienThiLaiLoaiPhong(string tenLoaiPhong)
		{
			return LoaiPhongDAL.Instance.hienThiLaiLoaiPhong(tenLoaiPhong);
		}
	}
}