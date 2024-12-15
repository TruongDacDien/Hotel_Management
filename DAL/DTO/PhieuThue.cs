using System;

namespace DAL.DTO
{
	public class PhieuThue
	{
		public int MaPhieuThue { get; set; }

		public int MaKH { get; set; }

		public DateTime NgayLapPhieu { get; set; }

		public int MaNV { get; set; }

		public bool IsDeleted { get; set; }
	}
}