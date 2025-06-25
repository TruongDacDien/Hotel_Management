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

		public static TienNghiBUS GetInstance()
		{
			if (instance == null) instance = new TienNghiBUS();
			return instance;
		}

		public List<TienNghi> getDataTienNghi()
		{
			return TienNghiDAL.GetInstance().getData();
		}

		public bool addTienNghi(TienNghi tn)
		{
			return TienNghiDAL.GetInstance().addTienNghi(tn);
		}

		public bool xoaTienNghi(TienNghi tn)
		{
			return TienNghiDAL.GetInstance().xoaTienNghi(tn);
		}

		public bool capNhatTienNghi(TienNghi tn)
		{
			return TienNghiDAL.GetInstance().capnhatTienNghi(tn);
		}

		public bool KiemTraTenTienNghi(TienNghi tn)
		{
			return TienNghiDAL.GetInstance().KiemTraTenTienNghi(tn);
		}

		public bool hienThiLaiTienNghi(string tenTN)
		{
			return TienNghiDAL.GetInstance().hienThiLaiTienNghi(tenTN);
		}

		public bool capNhatSoLuongTienNghi(int maTN, int soLuongThayDoi)
		{
			return TienNghiDAL.GetInstance().capNhatSoLuongTienNghi(maTN, soLuongThayDoi);
		}
	}
}