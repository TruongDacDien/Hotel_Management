using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class HoaDonBUS
	{
		private static HoaDonBUS Instance;

		private HoaDonBUS()
		{
		}

		public static HoaDonBUS GetInstance()
		{
			if (Instance == null) Instance = new HoaDonBUS();
			return Instance;
		}

		public List<HoaDon> GetHoaDons()
		{
			return HoaDonDAL.GetInstance().LayDuLieuHoaDon();
		}

		public HoaDon LayHoaDonThepMaCTPT(int? maCTPT)
		{
			return HoaDonDAL.GetInstance().LayHoaDonTheoMaCTPT(maCTPT);
		}

		public bool themHoaDon(HoaDon hd, out string error)
		{
			return HoaDonDAL.GetInstance().themHoaDon(hd, out error);
		}

		public int layMaHDMoiNhat()
		{
			return HoaDonDAL.GetInstance().layMaHDMoiNhat();
		}
		public bool capNhatHoaDon(HoaDon hoaDon)
		{
			return HoaDonDAL.GetInstance().capNhatHoaDon(hoaDon);
		}
	}
}