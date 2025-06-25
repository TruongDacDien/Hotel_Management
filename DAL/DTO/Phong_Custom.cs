using System;

namespace DAL.DTO
{
	public class Phong_Custom
	{
		private bool? isDay;

		public string MaPhong { get; set; }

		public string TinhTrang { get; set; }

		public string TenKH { get; set; }

		public string DonDep { get; set; }

		public string LoaiPhong { get; set; }

		public DateTime? NgayDen { get; set; }

		public int? SoNguoi { get; set; }

		public int? SoNgayO { get; set; }

		public int? MaCTPT { get; set; }

		public DateTime? NgayDi { get; set; }

		public decimal? SoGio { get; set; }

		public bool IsDay
		{
			get =>
				// Trả về false nếu SoGio là null hoặc nhỏ hơn 24
				SoGio.HasValue && SoGio.Value >= 24;
			set => isDay = value;
		}
	}
}