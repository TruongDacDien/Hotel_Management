namespace DAL.DTO
{
	public class TaiKhoanKH
	{
		public int MaTKKH { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public int MaKH { get; set; }

		public string AvatarId { get; set; }

		public string AvatarURL { get; set; }

		public string Email { get; set; }

		public DateTime LastLogin { get; set; }

		public bool Disabled { get; set; }

		public string MaXacNhan { get; set; }

		public DateTime ThoiGianHetHan { get; set; }

		public KhachHang KhachHang { get; set; }
	}
}
