namespace DAL.DTO
{
	public class DichVu
	{
		public string TenDV { get; set; }

		public string TenLoaiDV { get; set; }

		public int MaLoaiDV { get; set; }

		public decimal Gia { get; set; }

		public int MaDV { get; set; }

		public bool IsDeleted { get; set; }
	}
}