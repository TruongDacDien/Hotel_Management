using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class DichVu
	{
		private int maDV;
		private string tenDV;
		private int maLoaiDV;
		private decimal gia;

		public int MaDV { get => maDV; set => maDV = value; }
		public string TenDV { get => tenDV; set => tenDV = value; }
		public int MaLoaiDV { get => maLoaiDV; set => maLoaiDV = value; }
		public decimal Gia { get => gia; set => gia = value; }
	}
}
