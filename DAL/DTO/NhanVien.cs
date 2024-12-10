using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class NhanVien
	{
		private int maNV;
		private string hoTen;
		private string chucVu;
		private string sDT;
		private string diaChi;
		private string cCCD;
		private DateTime nTNS;
		private string gioiTinh;
		private decimal luong;
		private bool isDeleted;

		public int MaNV { get => maNV; set => maNV = value; }
		public string HoTen { get => hoTen; set => hoTen = value; }
		public string ChucVu { get => chucVu; set => chucVu = value; }
		public string SDT { get => sDT; set => sDT = value; }
		public string DiaChi { get => diaChi; set => diaChi = value; }
		public string CCCD { get => cCCD; set => cCCD = value; }
		public DateTime NTNS { get => nTNS; set => nTNS = value; }
		public string GioiTinh { get => gioiTinh; set => gioiTinh = value; }
		public decimal Luong { get => luong; set => luong = value; }
		public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
		public string DisplayInfo => $"{MaNV} - {HoTen}";
	}
}
