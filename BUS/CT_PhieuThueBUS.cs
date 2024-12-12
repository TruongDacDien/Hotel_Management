using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Data;

namespace BUS
{
    public class CT_PhieuThueBUS
    {
        private static CT_PhieuThueBUS Instance;

        private CT_PhieuThueBUS()
        {

        }

        public static CT_PhieuThueBUS GetInstance()
        {
            if (Instance == null)
            {
                Instance = new CT_PhieuThueBUS();
            }
            return Instance;
        }
        public bool addCTPhieuThue(CT_PhieuThue ctpt , out string error)
        {
            return CT_PhieuThueDAL.GetInstance().addCTPhieuThue(ctpt, out error);
        }
        public List<CT_PhieuThue> getCTPhieuThueTheoMaPT(int maPT)
        {
            return CT_PhieuThueDAL.GetInstance().getPhieuThueTheoMaPT(maPT);
        }

		public CT_PhieuThue getCTPhieuThueTheoMaCTPT(int? maCTPT)
		{
			return CT_PhieuThueDAL.GetInstance().getCT_PhieuThueTheoMaCTPT(maCTPT);
		}

		public bool suaTinhTrangThuePhong(int? maCTPT, string tinhtrangthuephong, out string error)
		{
			return CT_PhieuThueDAL.GetInstance().suaTinhTrangThuePhong(maCTPT, tinhtrangthuephong, out error);
		}

		public bool capNhatNgayBD(int? maCTPT, DateTime ngayBD, out string error)
		{
			return CT_PhieuThueDAL.GetInstance().capNhatNgayBD(maCTPT, ngayBD, out error);
		}

		public bool capNhatNgayKT(int? maCTPT, DateTime ngayKT, out string error)
		{
			return CT_PhieuThueDAL.GetInstance().capNhatNgayKT(maCTPT, ngayKT, out error);
		}

		public bool capNhatTien(int? maCTPT, decimal? tienPhong, out string errorCapNhatCTPT)
		{
			return CT_PhieuThueDAL.GetInstance().capNhatTien(maCTPT, tienPhong, out errorCapNhatCTPT);
		}
	}
}
