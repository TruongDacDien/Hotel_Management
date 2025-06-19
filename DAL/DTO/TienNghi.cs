namespace DAL.DTO
{
	public class TienNghi
	{
		public int MaTN { get; set; }

		public string TenTN { get; set; }

		public string ImageId { get; set; }

		public string ImageURL { get; set; }

		public int SoLuong { get; set; }

		public bool IsDeleted { get; set; }
	}
}