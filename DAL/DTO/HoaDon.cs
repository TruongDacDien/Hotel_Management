using System;

namespace DAL.DTO
{
	public class HoaDon
	{
		public int MaHD { get; set; }

		public int MaNV { get; set; }

		public int? MaCTPT { get; set; }

		public DateTime NgayLap { get; set; }

		public decimal TongTien { get; set; }

		public NhanVien NhanVien { get; set; }

		public CT_PhieuThue CT_PhieuThue { get; set; }
	}
}