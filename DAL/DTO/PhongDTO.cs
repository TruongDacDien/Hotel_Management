using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class PhongDTO: Phong
	{
		private string loaiPhong;

		public string LoaiPhong { get => loaiPhong; set => loaiPhong = value; }
	}
}
