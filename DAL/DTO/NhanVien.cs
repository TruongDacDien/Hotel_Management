using System;

namespace DAL.DTO
{
	public class NhanVien
	{
		public int MaNV { get; set; }

		public string HoTen { get; set; }

		public string ChucVu { get; set; }

		public string SDT { get; set; }

		public string DiaChi { get; set; }

		public string CCCD { get; set; }

		public DateTime NTNS { get; set; }

		public string GioiTinh { get; set; }

		public decimal Luong { get; set; }

		public bool IsDeleted { get; set; }

		public string DisplayInfo => $"{MaNV} - {HoTen}";
	}
}