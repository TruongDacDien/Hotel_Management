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
	///     Interaction logic for uc_QuanLyDichVu.xaml
	/// </summary>
	public partial class uc_QuanLyDichVu : UserControl
	{
		private ObservableCollection<DichVu> list;
		private CollectionView view;

		public uc_QuanLyDichVu()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<DichVu>(DichVuBUS.GetInstance().getDichVu());
			lsvDichVu.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(list);
			view.Filter = DichVuFilter;
		}

		private bool DichVuFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text))
				return true;
			return (obj as DichVu).TenDV.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private void nhanData(DichVu dv)
		{
			if (!DichVuBUS.GetInstance().KiemTraTrungTen(dv))
			{
				if (DichVuBUS.GetInstance().ThemDichVu(dv))
				{
					new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
			else
			{
				if (DichVuBUS.GetInstance().hienThiLaiDichVu(dv.TenDV))
				{
					new DialogCustoms("Tên dịch vụ đã tồn tại trong hệ thống\nĐã cập nhật lại danh sách", "Thông báo",
						DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
		}

		private void capNhatData(DichVu dv)
		{
			if (DichVuBUS.GetInstance().capNhatDichVu(dv))
			{
				new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiDanhSach();
			}
		}

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(lsvDichVu.ItemsSource).Refresh();
		}

		private void btnXoa_Click(object sender, RoutedEventArgs e)
		{
			var dv = (sender as Button).DataContext as DichVu;

			var thongbao = new DialogCustoms("Bạn có thật sự muốn xóa " + dv.TenDV, "Thông báo", DialogCustoms.YesNo);

			if (thongbao.ShowDialog() == true)
			{
				new DialogCustoms("Xoá thành công", "Thông báo", DialogCustoms.OK).Show();
				DichVuBUS.GetInstance().xoaDataDichVu(dv);
				TaiDanhSach();
			}
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			var ThemDichVu = new Them_SuaDichVu(false);
			ThemDichVu.truyen = nhanData;
			ThemDichVu.ShowDialog();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			var dv = (sender as Button).DataContext as DichVu;
			var CapNhatDichVu = new Them_SuaDichVu(true, dv);
			CapNhatDichVu.sua = capNhatData;
			CapNhatDichVu.ShowDialog();
		}
	}
}