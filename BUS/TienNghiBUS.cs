using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class TienNghiBUS
	{
		private static TienNghiBUS instance;

		private TienNghiBUS()
		{
		}

		public static TienNghiBUS Instance
		{
			get
			{
				if (instance == null) instance = new TienNghiBUS();
				return instance;
			}
		}

		public List<TienNghi> getDataTienNghi()
		{
			return TienNghiDAL.Instance.getData();
		}

		public bool addTienNghi(TienNghi tn)
		{
			return TienNghiDAL.Instance.addTienNghi(tn);
		}

		public bool xoaTienNghi(TienNghi tn)
		{
			return TienNghiDAL.Instance.xoaTienNghi(tn);
		}

		public bool capNhatTienNghi(TienNghi tn)
		{
			return TienNghiDAL.Instance.capnhatTienNghi(tn);
		}

		public bool KiemTraTenTienNghi(TienNghi tn)
		{
			return TienNghiDAL.Instance.KiemTraTenTienNghi(tn);
		}

		public bool hienThiLaiTienNghi(string tenTN)
		{
			return TienNghiDAL.Instance.hienThiLaiTienNghi(tenTN);
		}
	}
}