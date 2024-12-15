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

		public static LoaiDichVuBUS Instance
		{
			get
			{
				if (instance == null) instance = new LoaiDichVuBUS();
				return instance;
			}
		}

		public List<LoaiDV> getDataLoaiDV()
		{
			return LoaiDichVuDAL.Instance.getData();
		}

		public bool addLoaiDV(LoaiDV loaiDV)
		{
			return LoaiDichVuDAL.Instance.addDataLoaiDV(loaiDV);
		}

		public bool xoaLoaiDV(LoaiDV loaiDV)
		{
			return LoaiDichVuDAL.Instance.xoaLoaiDV(loaiDV);
		}

		public bool capNhatDataLoaiDV(LoaiDV loaiDV)
		{
			return LoaiDichVuDAL.Instance.capnhatLoaiDV(loaiDV);
		}

		public bool KiemTraTenLoaiDV(LoaiDV loaiDV)
		{
			return LoaiDichVuDAL.Instance.KiemTraTenLoaiDichVu(loaiDV);
		}

		public bool hienThiLaiLoaiDV(string tenLoaiDV)
		{
			return LoaiDichVuDAL.Instance.hienThiLaiLoaiDV(tenLoaiDV);
		}
	}
}