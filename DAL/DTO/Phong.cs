using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class Phong
	{
		private string soPhong;
		private int maLoaiPhong;
		private string tinhTrang;
		private string loaiPhong;
		private bool isDeleted;

		public string SoPhong { get => soPhong; set => soPhong = value; }
		public int MaLoaiPhong { get => maLoaiPhong; set => maLoaiPhong = value; }
		public string TinhTrang { get => tinhTrang; set => tinhTrang = value; }
		public string LoaiPhong { get => loaiPhong; set => loaiPhong = value; }
		public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
	}
}
