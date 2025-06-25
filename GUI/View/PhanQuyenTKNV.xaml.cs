using BUS;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for PhanQuyenTKNV.xaml
    /// </summary>
    public partial class PhanQuyenTKNV : Window
    {
		private PhanQuyen phanQuyen;

        public PhanQuyenTKNV()
        {
            InitializeComponent();
        }

		public PhanQuyenTKNV(TaiKhoanNV taiKhoanNV): this()
		{
			phanQuyen = TaiKhoanNVBUS.GetInstance().layPhanQuyenTaiKhoan(taiKhoanNV.MaTKNV);
			ckb_TrangChu.IsChecked = phanQuyen.TrangChu;
			ckb_Phong.IsChecked = phanQuyen.Phong;
			ckb_DatPhong.IsChecked= phanQuyen.DatPhong;
			ckb_DatDV.IsChecked = phanQuyen.QLDatDV;
			ckb_HoaDon.IsChecked = phanQuyen.HoaDon;
			ckb_QLKhachHang.IsChecked = phanQuyen.QLKhachHang;
			ckb_QLTaiKhoanKH.IsChecked = phanQuyen.QLTaiKhoanKH;
			ckb_QLPhong.IsChecked = phanQuyen.QLPhong;
			ckb_QLLoaiPhong.IsChecked = phanQuyen.QLLoaiPhong;
			ckb_QLDichVu.IsChecked = phanQuyen.QLDichVu;
			ckb_QLLoaiDV.IsChecked = phanQuyen.QLLoaiDichVu;
			ckb_TienNghi.IsChecked = phanQuyen.QLTienNghi;
			ckb_QLNhanVien.IsChecked = phanQuyen.QLNhanVien;
			ckb_QLTaiKhoanNV.IsChecked = phanQuyen.QLTaiKhoanNV;
			ckb_ThongKe.IsChecked = phanQuyen.ThongKe;
			ckb_DiaDiemXQ.IsChecked = phanQuyen.QLDDXQ;
		}

		private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (phanQuyen == null || sender is not CheckBox chk) return;

			bool isChecked = chk.IsChecked == true;

			switch (chk.Name)
			{
				case "ckb_TrangChu": phanQuyen.TrangChu = isChecked; break;
				case "ckb_Phong": phanQuyen.Phong = isChecked; break;
				case "ckb_DatPhong": phanQuyen.DatPhong = isChecked; break;
				case "ckb_DatDV": phanQuyen.QLDatDV = isChecked; break;
				case "ckb_HoaDon": phanQuyen.HoaDon = isChecked; break;
				case "ckb_QLKhachHang": phanQuyen.QLKhachHang = isChecked; break;
				case "ckb_QLTaiKhoanKH": phanQuyen.QLTaiKhoanKH = isChecked; break;
				case "ckb_QLPhong": phanQuyen.QLPhong = isChecked; break;
				case "ckb_QLLoaiPhong": phanQuyen.QLLoaiPhong = isChecked; break;
				case "ckb_QLDichVu": phanQuyen.QLDichVu = isChecked; break;
				case "ckb_QLLoaiDV": phanQuyen.QLLoaiDichVu = isChecked; break;
				case "ckb_TienNghi": phanQuyen.QLTienNghi = isChecked; break;
				case "ckb_QLNhanVien": phanQuyen.QLNhanVien = isChecked; break;
				case "ckb_QLTaiKhoanNV": phanQuyen.QLTaiKhoanNV = isChecked; break;
				case "ckb_ThongKe": phanQuyen.ThongKe = isChecked; break;
				case "ckb_DiaDiemXQ": phanQuyen.QLDDXQ = isChecked; break;
			}
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (TaiKhoanNVBUS.GetInstance().capNhatPhanQuyenTaiKhoan(phanQuyen))
			{
				new DialogCustoms("Cập nhật phân quyền tài khoản thành công!", "Thông báo", DialogCustoms.OK).Show();
			}
			else
			{
				new DialogCustoms("Lỗi cập nhật phân quyền tài khoản!", "Thông báo", DialogCustoms.OK).Show();
				return;
			}
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
