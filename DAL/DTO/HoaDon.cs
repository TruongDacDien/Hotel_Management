using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class HoaDon
	{
		private int maHD;
		private int maNV;
		private int maCTPT;
		private DateTime ngayLap;
		private decimal tongTien;

		public int MaHD { get => maHD; set => maHD = value; }
		public int MaNV { get => maNV; set => maNV = value; }
		public int MaCTPT { get => maCTPT; set => maCTPT = value; }
		public DateTime NgayLap { get => ngayLap; set => ngayLap = value; }
		public decimal TongTien { get => tongTien; set => tongTien = value; }
	}
}
