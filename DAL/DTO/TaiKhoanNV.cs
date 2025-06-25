namespace DAL.DTO
{
	public class TaiKhoanNV
	{
		public int MaTKNV { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public int MaNV { get; set; }

		public string AvatarId { get; set; }

		public string AvatarURL { get; set; }

		public string Email { get; set; }

		public DateTime LastLogin { get; set; }

		public bool Disabled { get; set; }

		public bool IsDeleted { get; set; }

		public string MaXacNhan { get; set; }

		public DateTime ThoiGianHetHan {  get; set; }

		public NhanVien NhanVien { get; set; }
	}
}