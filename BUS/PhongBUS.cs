using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Data;

namespace BUS
{
    public class PhongBUS
    {
        private static PhongBUS Instance;

        private PhongBUS()
        {

        }

        public static PhongBUS GetInstance()
        {
            if (Instance == null)
            {
                Instance = new PhongBUS();
            }
            return Instance;
        }
        

        public List<Phong_Custom> getDataPhongCustomTheoNgay(DateTime? ngayChon)
        {
            return PhongDAL.GetInstance().getDataPhongTheoNgay(ngayChon);
        }

        public List<PhongTrong> getPhongTrong(DateTime? ngayBD, DateTime? ngayKT)
        {
            return PhongDAL.GetInstance().getPhongTrong(ngayBD,ngayKT);
        }
		public decimal? tinhTienPhong(Phong_Custom phong, CT_PhieuThue cT_PhieuThue)
		{
			// Lấy giá tiền theo ngày và giờ
			decimal? giaNgay = PhongDAL.GetInstance().layGiaTienTheoMaPhong(phong.MaPhong, true); // Giá theo ngày
			decimal? giaGio = PhongDAL.GetInstance().layGiaTienTheoMaPhong(phong.MaPhong, false); // Giá theo giờ

			if (giaNgay == null || giaGio == null)
			{
				// Trường hợp không tìm thấy giá tiền, trả về null
				return null;
			}

			// Nếu ở theo ngày
			if (phong.IsDay)
			{
				// Tính số ngày nguyên và số giờ dư
				TimeSpan duration = cT_PhieuThue.NgayKT - cT_PhieuThue.NgayBD;
				int soNgay = (int)duration.TotalDays; // Số ngày nguyên
				decimal soGioLe = (decimal)(duration.TotalHours % 24); // Số giờ dư
				int soGioLamTron = soGioLe > 0 ? (int)Math.Ceiling(soGioLe) : 0; // Làm tròn giờ dư

				// Tính tổng tiền
				return (soNgay * giaNgay) + (soGioLamTron * giaGio);
			}
			else
			{
				// Tính tiền theo giờ (dựa trên khoảng thời gian)
				TimeSpan duration = cT_PhieuThue.NgayKT - cT_PhieuThue.NgayBD;
				decimal soPhut = (decimal)duration.TotalMinutes; // Tổng số phút

				// Làm tròn theo chính sách: nếu thời gian nhỏ hơn 1h thì làm tròn lên 1h
				int soGioLamTron = soPhut > 0 ? Math.Max(1, (int)Math.Ceiling(soPhut / 60)) : 0;

				// Tính tổng tiền
				return soGioLamTron * giaGio;
			}
		}
		public decimal layTienPhongTheoSoPhong(Phong_Custom phong)
        {
            return PhongDAL.GetInstance().layGiaTienTheoMaPhong(phong.MaPhong, phong.IsDay);
        }

        public bool suaTinhTrangDonDep(string maPhong, string text, out string error)
        {
            return PhongDAL.GetInstance().suaTinhTrangPhong(maPhong, text, out error);
        }

        public List<Phong> getDataPhong()
        {
            return PhongDAL.GetInstance().getPhong();
        }

        public bool addDataPhong(Phong phong)
        {
            return PhongDAL.GetInstance().addDataPhong(phong);
        }

        public bool capNhatDataPhong(Phong phong)
        {
            return PhongDAL.GetInstance().capNhatPhong(phong);
        }

        public void xoaDataPhong(Phong phong)
        {
            PhongDAL.GetInstance().xoaThongTinPhong(phong);
        }

        public decimal layTienPhongTheoSoPhong(string soPhong, bool isDay)
        {
            return PhongDAL.GetInstance().layGiaTienTheoMaPhong(soPhong,isDay);
        }

		public bool hienThiLaiPhong(string soPhong)
		{
			return PhongDAL.GetInstance().hienThiLaiPhong(soPhong);
		}
	}
}
