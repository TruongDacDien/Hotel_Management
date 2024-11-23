using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class PhanQuyen
	{
		private int maPQ;
		private bool trangChu;
		private bool phong;
		private bool datPhong;
		private bool hoaDon;
		private bool qLKhachHang;
		private bool qLPhong;
		private bool qLLoaiPhong;
		private bool qLDichVu;
		private bool qLLoaiDV;
		private bool qLTienNghi;
		private bool qLCTTienNghi;
		private bool qLTaiKhoan;
		private bool qLNhanVien;
		private bool thongKe;

		public int MaPQ { get => maPQ; set => maPQ = value; }
		public bool TrangChu { get => trangChu; set => trangChu = value; }
		public bool Phong { get => phong; set => phong = value; }
		public bool DatPhong { get => datPhong; set => datPhong = value; }
		public bool HoaDon { get => hoaDon; set => hoaDon = value; }
		public bool QLKhachHang { get => qLKhachHang; set => qLKhachHang = value; }
		public bool QLPhong { get => qLPhong; set => qLPhong = value; }
		public bool QLLoaiPhong { get => qLLoaiPhong; set => qLLoaiPhong = value; }
		public bool QLDichVu { get => qLDichVu; set => qLDichVu = value; }
		public bool QLLoaiDV { get => qLLoaiDV; set => qLLoaiDV = value; }
		public bool QLTienNghi { get => qLTienNghi; set => qLTienNghi = value; }
		public bool QLCTTienNghi { get => qLCTTienNghi; set => qLCTTienNghi = value; }
		public bool QLTaiKhoan { get => qLTaiKhoan; set => qLTaiKhoan = value; }
		public bool QLNhanVien { get => qLNhanVien; set => qLNhanVien = value; }
		public bool ThongKe { get => thongKe; set => thongKe = value; }
	}
}
