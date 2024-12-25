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
	///     Interaction logic for uc_QuanLyTaiKhoan.xaml
	/// </summary>
	public partial class uc_QuanLyTaiKhoan : UserControl
	{
		private ObservableCollection<TaiKhoan> list;
		private CollectionView view;

		public uc_QuanLyTaiKhoan()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<TaiKhoan>(TaiKhoanBUS.GetInstance().getDataTaiKhoan());
			lsvTaiKhoan.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(list);
			view.Filter = TaiKhoanFilter;
		}

		private bool TaiKhoanFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text))
				return true;
			return (obj as TaiKhoan).Username.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private void nhanData(TaiKhoan taiKhoan)
		{
			if (!TaiKhoanBUS.GetInstance().kiemTraTrungUsername(taiKhoan.Username))
			{
				if (TaiKhoanBUS.GetInstance().themTaiKhoan(taiKhoan))
				{
					new DialogCustoms("Thêm tài khoản thành công", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
			else
			{
				if (TaiKhoanBUS.GetInstance().hienThiLaiTaiKhoan(taiKhoan.Username))
				{
					new DialogCustoms("Tên tài khoản đã tồn tại trong hệ thống", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
		}

		private void capNhatData(TaiKhoan taiKhoan)
		{
			if (TaiKhoanBUS.GetInstance().capNhatTaiKhoan(taiKhoan))
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
			var taiKhoan = (sender as Button).DataContext as TaiKhoan;

			var thongbao = new DialogCustoms("Bạn có thật sự muốn xóa " + taiKhoan.Username, "Thông báo",
				DialogCustoms.YesNo);

			if (thongbao.ShowDialog() == true)
			{
				new DialogCustoms("Xoá thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiKhoanBUS.GetInstance().xoaTaiKhoan(taiKhoan);
				TaiDanhSach();
			}
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			var ThemTaiKhoan = new Them_SuaTaiKhoan();
			ThemTaiKhoan.truyenTaiKhoan = nhanData;
			ThemTaiKhoan.ShowDialog();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			var taiKhoan = (sender as Button).DataContext as TaiKhoan;
			var CapNhatTaiKhoan = new Them_SuaTaiKhoan(true, taiKhoan);
			CapNhatTaiKhoan.suaTaiKhoan = capNhatData;
			CapNhatTaiKhoan.ShowDialog();
		}
	}
}