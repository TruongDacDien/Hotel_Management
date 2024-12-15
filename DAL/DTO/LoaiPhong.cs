namespace DAL.DTO
{
	public class LoaiPhong
	{
		public int MaLoaiPhong { get; set; }

		public string TenLoaiPhong { get; set; }

		public int SoNguoiToiDa { get; set; }

		public decimal GiaNgay { get; set; }

		public decimal GiaGio { get; set; }

		public bool IsDeleted { get; set; }
	}
}