namespace DAL.DTO
{
	public class CT_TienNghi
	{
		public int? MaCTTN { get; set; }

		public int MaTN { get; set; }

		public string SoPhong { get; set; }

		public int SL { get; set; }

		public string TenTN { get; set; }

		public bool IsDeleted { get; set; }
	}
}