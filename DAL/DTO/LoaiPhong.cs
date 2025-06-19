using System.Collections.Generic;
using System.Text.Unicode;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.DTO
{
	public class LoaiPhong
	{
		public int MaLoaiPhong { get; set; }

		public string TenLoaiPhong { get; set; }

		public string MoTa { get; set; }

		public string ChinhSach { get; set; }

		public string ChinhSachHuy { get; set; }

		public int SoNguoiToiDa { get; set; }

		public decimal GiaNgay { get; set; }

		public decimal GiaGio { get; set; }

		public string ImageId { get; set; }

		public string ImageURL { get; set; }

		public bool IsDeleted { get; set; }
	}
}