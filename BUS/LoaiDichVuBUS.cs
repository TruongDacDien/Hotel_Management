using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class LoaiDichVuBUS
	{
		private static LoaiDichVuBUS instance;

		private LoaiDichVuBUS()
		{
		}

		public static LoaiDichVuBUS GetInstance()
		{
			if (instance == null) instance = new LoaiDichVuBUS();
			return instance;
		}

		public List<LoaiDV> getDataLoaiDV()
		{
			return LoaiDichVuDAL.GetInstance().getData();
		}

		public bool addLoaiDV(LoaiDV loaiDV)
		{
			return LoaiDichVuDAL.GetInstance().addDataLoaiDV(loaiDV);
		}

		public bool xoaLoaiDV(LoaiDV loaiDV)
		{
			return LoaiDichVuDAL.GetInstance().xoaLoaiDV(loaiDV);
		}

		public bool capNhatDataLoaiDV(LoaiDV loaiDV)
		{
			return LoaiDichVuDAL.GetInstance().capnhatLoaiDV(loaiDV);
		}

		public bool KiemTraTenLoaiDV(LoaiDV loaiDV)
		{
			return LoaiDichVuDAL.GetInstance().KiemTraTenLoaiDichVu(loaiDV);
		}

		public bool hienThiLaiLoaiDV(string tenLoaiDV)
		{
			return LoaiDichVuDAL.GetInstance().hienThiLaiLoaiDV(tenLoaiDV);
		}
	}
}