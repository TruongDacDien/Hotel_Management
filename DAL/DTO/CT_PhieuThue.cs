using System;

namespace DAL.DTO
{
	public class CT_PhieuThue
	{
		public int MaCTPT { get; set; }

		public int MaPhieuThue { get; set; }

		public string SoPhong { get; set; }

		public DateTime NgayBD { get; set; }

		public DateTime NgayKT { get; set; }

		public DateTime? ThoiDiemCheckIn { get; set; }

		public string NoiDungCheckIn { get; set; }

		public DateTime? ThoiDiemCheckOut { get; set; }

		public string NoiDungCheckOut { get; set; }

		public int SoNguoiO { get; set; }

		public string TinhTrangThue { get; set; }

		public decimal TienPhong { get; set; }

		public bool? DaThanhToan { get; set; }

		public string HinhThucThanhToan { get; set; }
	}
}
