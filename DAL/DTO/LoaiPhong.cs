using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class LoaiPhong
	{
		private int maLoaiPhong;
		private string tenLoaiPhong;
		private int soNguoiToiDa;
		private decimal giaNgay;
		private decimal giaGio;
		private bool isDeleted;

		public int MaLoaiPhong { get => maLoaiPhong; set => maLoaiPhong = value; }
		public string TenLoaiPhong { get => tenLoaiPhong; set => tenLoaiPhong = value; }
		public int SoNguoiToiDa { get => soNguoiToiDa; set => soNguoiToiDa = value; }
		public decimal GiaNgay { get => giaNgay; set => giaNgay = value; }
		public decimal GiaGio { get => giaGio; set => giaGio = value; }
		public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
	}
}
