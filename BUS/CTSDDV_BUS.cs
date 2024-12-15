using System.Collections.Generic;
using System.Linq;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class CTSDDV_BUS
	{
		private static CTSDDV_BUS Instance;

		private CTSDDV_BUS()
		{
		}

		public static CTSDDV_BUS GetInstance()
		{
			if (Instance == null) Instance = new CTSDDV_BUS();
			return Instance;
		}

		public bool addDataCTSDDC(CT_SDDichVu ctsddv, out string error)
		{
			return CTSDDV_DAL.GetInstance().addDataCTSDDC(ctsddv, out error);
		}

		public List<DichVu_DaChon> getCTSDDVtheoMaCTPT(int? maCTPT)
		{
			return CTSDDV_DAL.GetInstance().getCTSDDVtheoMaCTPT(maCTPT);
		}

		public decimal? tinhTongTienDichVuTheoMaCTPT(int? maCTPT)
		{
			var ls = CTSDDV_DAL.GetInstance().tongTienChiTietSuDungDichVu(maCTPT);
			if (ls.Count == 0) return 0;

			return ls.Sum();
		}
	}
}