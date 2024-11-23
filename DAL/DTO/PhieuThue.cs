using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class PhieuThue
	{
		private int maPhieuThue;
		private int maKH;
		private DateTime ngayLapPhieu;
		private int maNV;

		public int MaPhieuThue { get => maPhieuThue; set => maPhieuThue = value; }
		public int MaKH { get => maKH; set => maKH = value; }
		public DateTime NgayLapPhieu { get => ngayLapPhieu; set => ngayLapPhieu = value; }
		public int MaNV { get => maNV; set => maNV = value; }
	}
}
