namespace DAL.DTO
{
	public class CT_SDDichVu
	{
		public int MaCTSDDV { get; set; }

		public int? MaCTPT { get; set; }

		public int MaKH { get; set; }

		public int? MaDV { get; set; }

		public int SL { get; set; }

		public decimal ThanhTien { get; set; }

		public System.DateTime NgayApDung { get; set; }

		public string TrangThai { get; set; }

		public bool? DaThanhToan { get; set; }

		public string HinhThucThanhToan { get; set; }

		public string TenKH { get; set; }

		public string TenDV { get; set; }

		public string SoPhong { get; set; }
	}
}
