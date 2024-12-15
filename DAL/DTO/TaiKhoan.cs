namespace DAL.DTO
{
	public class TaiKhoan
	{
		public string Username { get; set; }

		public string Password { get; set; }

		public int MaNV { get; set; }

		public int CapDoQuyen { get; set; }

		public byte[] Avatar { get; set; }

		public NhanVien NhanVien { get; set; }

		public bool Disabled { get; set; }
	}
}