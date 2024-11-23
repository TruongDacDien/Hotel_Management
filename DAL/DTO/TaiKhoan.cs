using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class TaiKhoan
	{
		private int maTK;
		private string username;
		private string password;
		private string email;
		private int maPQ;
		private byte[] avatar;

		public int MaTK { get => maTK; set => maTK = value; }
		public string Username { get => username; set => username = value; }
		public string Password { get => password; set => password = value; }
		public string Email { get => email; set => email = value; }
		public int MaPQ { get => maPQ; set => maPQ = value; }
		public byte[] Avatar { get => avatar; set => avatar = value; }
	}
}
