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
	///     Interaction logic for uc_QuanLyLoaiDichVu.xaml
	/// </summary>
	public partial class uc_QuanLyLoaiDichVu : UserControl
	{
		private ObservableCollection<LoaiDV> list;
		private CollectionView view;

		public uc_QuanLyLoaiDichVu()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		#region Event

		private void btnThemLoaiDV_Click(object sender, RoutedEventArgs e)
		{
			var themLoaiDichVu = new Them_SuaLoaiDichVu();
			themLoaiDichVu.truyenLoaiDV = nhanData;
			themLoaiDichVu.ShowDialog();
		}

		private void btnSuaLoaiDV_Click(object sender, RoutedEventArgs e)
		{
			var loaiDV = (sender as Button).DataContext as LoaiDV;
			var capNhatLoaiDichVu = new Them_SuaLoaiDichVu(loaiDV);
			capNhatLoaiDichVu.suaLoaiDV = capNhatData;
			capNhatLoaiDichVu.ShowDialog();
		}

		private void btnXoaLoaiDV_Click(object sender, RoutedEventArgs e)
		{
			var loaiDV = (sender as Button).DataContext as LoaiDV;
			var ThongBao = new DialogCustoms("Bạn có thật sự muốn xóa loại dịch vụ " + loaiDV.TenLoaiDV, "Thông báo",
				DialogCustoms.YesNo);
			if (ThongBao.ShowDialog() == true)
			{
				new DialogCustoms("Xóa thành công", "Thông báo", DialogCustoms.OK).Show();
				LoaiDichVuBUS.Instance.xoaLoaiDV(loaiDV);
				TaiDanhSach();
			}
		}

		#endregion

		#region Method

		private bool LoaiDVFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text))
				return true;
			return (obj as LoaiDV).TenLoaiDV.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<LoaiDV>(LoaiDichVuBUS.Instance.getDataLoaiDV());
			lsvLoaiDV.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(list);
			view.Filter = LoaiDVFilter;
		}

		private void nhanData(LoaiDV loaiDV)
		{
			if (!LoaiDichVuBUS.Instance.KiemTraTenLoaiDV(loaiDV))
			{
				if (LoaiDichVuBUS.Instance.addLoaiDV(loaiDV))
				{
					new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
			else
			{
				if (LoaiDichVuBUS.Instance.hienThiLaiLoaiDV(loaiDV.TenLoaiDV))
				{
					new DialogCustoms("Tên loại dịch vụ đã tồn tại", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
		}

		private void capNhatData(LoaiDV loaiDV)
		{
			if (!LoaiDichVuBUS.Instance.KiemTraTenLoaiDV(loaiDV))
			{
				if (LoaiDichVuBUS.Instance.capNhatDataLoaiDV(loaiDV))
				{
					new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
			else
			{
				new DialogCustoms("Tên loại dịch vụ đã tồn tại", "Thông báo", DialogCustoms.OK).Show();
			}
		}

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(lsvLoaiDV.ItemsSource).Refresh();
		}

		#endregion
	}
}