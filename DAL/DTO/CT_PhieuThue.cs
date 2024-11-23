using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class CT_PhieuThue
	{
		private int maCTPT;
		private int maPhieuThue;
		private string soPhong;
		private DateTime ngayBD;
		private DateTime ngayKT;
		private int soNguoiO;
		private string tinhTrangThue;
		private decimal tienPhong;
		private DateTime ngayTraThucTe;

		public int MaCTPT { get => maCTPT; set => maCTPT = value; }
		public int MaPhieuThue { get => maPhieuThue; set => maPhieuThue = value; }
		public string SoPhong { get => soPhong; set => soPhong = value; }
		public DateTime NgayBD { get => ngayBD; set => ngayBD = value; }
		public DateTime NgayKT { get => ngayKT; set => ngayKT = value; }
		public int SoNguoiO { get => soNguoiO; set => soNguoiO = value; }
		public string TinhTrangThue { get => tinhTrangThue; set => tinhTrangThue = value; }
		public decimal TienPhong { get => tienPhong; set => tienPhong = value; }
		public DateTime NgayTraThucTe { get => ngayTraThucTe; set => ngayTraThucTe = value; }
	}
}
