namespace DAL.DTO
{
	public class DichVu
	{
		public int MaDV { get; set; }

		public string TenDV { get; set; }

		public string MoTa { get; set; }

		public int MaLoaiDV { get; set; }

		public string TenLoaiDV { get; set; }

		public decimal Gia { get; set; }

		public int SoLuong { get; set; }

		public string ImageId { get; set; }

		public string ImageURL { get; set; }

		public bool IsDeleted { get; set; }

	}
}