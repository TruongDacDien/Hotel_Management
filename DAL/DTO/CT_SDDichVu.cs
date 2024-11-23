using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class CT_SDDichVu
	{
		private int maCTSDDV;
		private int maCTPT;
		private int maDV;
		private int sL;
		private decimal thanhTien;

		public int MaCTSDDV { get => maCTSDDV; set => maCTSDDV = value; }
		public int MaCTPT { get => maCTPT; set => maCTPT = value; }
		public int MaDV { get => maDV; set => maDV = value; }
		public int SL { get => sL; set => sL = value; }
		public decimal ThanhTien { get => thanhTien; set => thanhTien = value; }
	}
}
