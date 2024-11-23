using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class LoaiDV
	{
		private int maLoaiDV;
		private string tenLoaiDV;

		public int MaLoaiDV { get => maLoaiDV; set => maLoaiDV = value; }
		public string TenLoaiDV { get => tenLoaiDV; set => tenLoaiDV = value; }
	}
}
