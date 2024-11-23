using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class KhachHang
	{
		private int maKH;
		private string tenKH;
		private string gioiTinh;
		private string cCCD;
		private string sDT;
		private string diaChi;
		private string quocTich;

		public int MaKH { get => maKH; set => maKH = value; }
		public string TenKH { get => tenKH; set => tenKH = value; }
		public string GioiTinh { get => gioiTinh; set => gioiTinh = value; }
		public string CCCD { get => cCCD; set => cCCD = value; }
		public string SDT { get => sDT; set => sDT = value; }
		public string DiaChi { get => diaChi; set => diaChi = value; }
		public string QuocTich { get => quocTich; set => quocTich = value; }
	}
}
