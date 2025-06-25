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
	///     Interaction logic for uc_QuanLyTienNghi.xaml
	/// </summary>
	public partial class uc_QuanLyTienNghi : UserControl
	{
		private ObservableCollection<TienNghi> list;
		private CollectionView view;

		public uc_QuanLyTienNghi()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<TienNghi>(TienNghiBUS.GetInstance().getDataTienNghi());
			lsvTienNghi.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(lsvTienNghi.ItemsSource);
			view.Filter = TienNghiFilter;
		}

		private bool TienNghiFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text))
				return true;
			return (obj as TienNghi).TenTN.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private void btnSuaTienNghi_Click(object sender, RoutedEventArgs e)
		{
			var tienNghi = (sender as Button).DataContext as TienNghi;
			if (tienNghi != null)
			{
				var CapNhatTienNghi = new Them_SuaTienNghi(tienNghi);
				CapNhatTienNghi.suaTN = capNhatData;
				CapNhatTienNghi.ShowDialog();
			}
		}

		private void btnXoaTienNghi_Click(object sender, RoutedEventArgs e)
		{
			var tn = (sender as Button).DataContext as TienNghi;
			var thongbao = new DialogCustoms("Bạn có thật sự muốn xóa tiện nghi " + tn.TenTN, "Thông báo",
				DialogCustoms.YesNo);

			if (thongbao.ShowDialog() == true)
			{
				new DialogCustoms("Xoá thành công", "Thông báo", DialogCustoms.OK).Show();
				TienNghiBUS.GetInstance().xoaTienNghi(tn);
				TaiDanhSach();
			}
		}

		private void btnThemTienNghi_Click(object sender, RoutedEventArgs e)
		{
			var ThemTienNghi = new Them_SuaTienNghi();
			ThemTienNghi.truyenTN = nhanData;
			ThemTienNghi.ShowDialog();
		}

		private void nhanData(TienNghi tn)
		{
			if (!TienNghiBUS.GetInstance().KiemTraTenTienNghi(tn))
			{
				if (TienNghiBUS.GetInstance().addTienNghi(tn))
				{
					new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).ShowDialog();
					TaiDanhSach();
				}
			}
			else
			{
				if (TienNghiBUS.GetInstance().hienThiLaiTienNghi(tn.TenTN))
				{
					new DialogCustoms("Tên tiện nghi đã tồn tại trong hệ thống\n Đã cập nhật lại danh sách",
						"Thông báo", DialogCustoms.OK).ShowDialog();
					TaiDanhSach();
				}
			}
		}

		private void capNhatData(TienNghi tn)
		{
			if (TienNghiBUS.GetInstance().capNhatTienNghi(tn))
			{
				new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).ShowDialog();
				TaiDanhSach();
			}
			else
			{
				new DialogCustoms("Cập nhật tiện nghi không thành công", "Thông báo", DialogCustoms.OK).ShowDialog();
			}
		}

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(lsvTienNghi.ItemsSource).Refresh();
		}
	}
}