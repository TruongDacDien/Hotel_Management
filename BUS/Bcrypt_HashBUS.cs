namespace BUS
{
	public class Bcrypt_HashBUS
	{
		private static Bcrypt_HashBUS Instance;

		private Bcrypt_HashBUS()
		{
		}

		public static Bcrypt_HashBUS GetInstance()
		{
			if (Instance == null) Instance = new Bcrypt_HashBUS();
			return Instance;
		}

		// Hàm mã hóa mật khẩu bằng Bcrypt
		public string HashMatKhau(string pass)
		{
			// Tự động thêm salt và mã hóa
			var hashedPassword = BCrypt.Net.BCrypt.HashPassword(pass);
			return hashedPassword;
		}

		// Hàm kiểm tra mật khẩu khớp với hash
		public bool VerifyMatKhau(string pass, string hashedPassword)
		{
			// Kiểm tra mật khẩu nhập vào với hash đã lưu
			return BCrypt.Net.BCrypt.Verify(pass, hashedPassword);
		}
	}
}