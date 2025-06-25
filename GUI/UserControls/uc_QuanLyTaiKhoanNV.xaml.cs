using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BUS;
using DAL.DTO;
using GUI.View;

namespace GUI.UserControls
{
	/// <summary>
	///     Interaction logic for uc_QuanLyTaiKhoanNV.xaml
	/// </summary>
	public partial class uc_QuanLyTaiKhoanNV : UserControl
	{
		private ObservableCollection<TaiKhoanNV> list;
		private CollectionView view;

		public uc_QuanLyTaiKhoanNV()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<TaiKhoanNV>(TaiKhoanNVBUS.GetInstance().getDataTaiKhoan());
			lsvTaiKhoan.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(list);
			view.Filter = TaiKhoanFilter;
		}

		private bool TaiKhoanFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text))
				return true;
			return (obj as TaiKhoanNV).Username.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private bool nhanData(TaiKhoanNV taiKhoan)
		{
			if (!TaiKhoanNVBUS.GetInstance().kiemTraTrungUsername(taiKhoan.Username))
			{
				if (TaiKhoanNVBUS.GetInstance().themTaiKhoan(taiKhoan))
				{
					new DialogCustoms("Thêm tài khoản thành công", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
					return true;
				}
			}
			else
			{
				if (TaiKhoanNVBUS.GetInstance().hienThiLaiTaiKhoan(taiKhoan.Username))
				{
					new DialogCustoms("Tên tài khoản đã tồn tại trong hệ thống", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
			return false;
		}

		private void capNhatData(TaiKhoanNV taiKhoan)
		{
			if (TaiKhoanNVBUS.GetInstance().capNhatTaiKhoan(taiKhoan))
			{
				new DialogCustoms("Cập nhật tài khoản thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiDanhSach();
			}
		}

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(lsvTaiKhoan.ItemsSource).Refresh();
		}

		private void btnXoa_Click(object sender, RoutedEventArgs e)
		{
			var taiKhoan = (sender as Button).DataContext as TaiKhoanNV;

			var thongbao = new DialogCustoms("Bạn có thật sự muốn xóa " + taiKhoan.Username, "Thông báo",
				DialogCustoms.YesNo);

			if (thongbao.ShowDialog() == true)
			{
				new DialogCustoms("Xoá thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiKhoanNVBUS.GetInstance().xoaTaiKhoan(taiKhoan);
				TaiDanhSach();
			}
		}

		private void btnPhanQuyen_Click(object sender, RoutedEventArgs e)
		{
			var taiKhoanNV = (sender as Button).DataContext as TaiKhoanNV;
			var PhanQuyenTaiKhoanNV = new PhanQuyenTKNV(taiKhoanNV);
			PhanQuyenTaiKhoanNV.ShowDialog();
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			var ThemTaiKhoan = new Them_SuaTaiKhoanNV();
			ThemTaiKhoan.truyenTaiKhoan = nhanData;
			ThemTaiKhoan.ShowDialog();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			var taiKhoan = (sender as Button).DataContext as TaiKhoanNV;
			var CapNhatTaiKhoan = new Them_SuaTaiKhoanNV(true, taiKhoan);
			CapNhatTaiKhoan.suaTaiKhoan = capNhatData;
			CapNhatTaiKhoan.ShowDialog();
		}
	}
}