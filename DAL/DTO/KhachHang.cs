namespace DAL.DTO
{
	public class KhachHang
	{
		public int MaKH { get; set; }

		public string TenKH { get; set; }

		public string GioiTinh { get; set; }

		public string CCCD { get; set; }

		public string CCCDImage { get; set; }

		public string SDT { get; set; }

		public string DiaChi { get; set; }

		public string QuocTich { get; set; }

		public bool IsDeleted { get; set; }

		public string DisplayInfo => $"{MaKH} - {TenKH}";
	}
}