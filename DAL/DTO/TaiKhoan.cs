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
		private string username;
		private string password;
		private int maNV;
		private int capDoQuyen;
		private byte[] avatar;
		private NhanVien nhanVien;
		private bool disabled;

		public string Username { get => username; set => username = value; }
		public string Password { get => password; set => password = value; }
		public int MaNV { get => maNV; set => maNV = value; }
		public int CapDoQuyen { get => capDoQuyen; set => capDoQuyen = value; }
		public byte[] Avatar { get => avatar; set => avatar = value; }
		public NhanVien NhanVien { get => nhanVien; set => nhanVien = value; }
		public bool Disabled { get => disabled; set => disabled = value; }
	}
}
