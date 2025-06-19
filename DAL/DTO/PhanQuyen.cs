namespace DAL.DTO
{
	public class PhanQuyen
	{
		public int MaPQ { get; set; }

		public int MaTKNV { get; set; }

		public bool TrangChu { get; set; }

		public bool Phong {get; set;}

		public bool DatPhong { get; set;}

		public bool HoaDon { get; set; }

		public bool QLKhachHang { get; set; }

		public bool QLPhong { get; set; }

		public bool QLLoaiPhong { get; set; }

		public bool QLDichVu { get; set; }

		public bool QLLoaiDichVu { get; set; }

		public bool QLTienNghi { get; set; }

		public bool QLNhanVien { get; set; }
			 
		public bool QLTaiKhoan { get; set; }

		public bool ThongKe { get; set; }

		public bool ThongBao { get; set; }

		public bool LichSuHoatDong { get; set; }

        public bool QLDatDV { get; set; }

        public bool QLDDXQ { get; set; }
    }
}