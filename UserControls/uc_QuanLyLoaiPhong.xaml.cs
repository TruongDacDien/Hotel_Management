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
	///     Interaction logic for uc_QuanLyLoaiPhong.xaml
	/// </summary>
	public partial class uc_QuanLyLoaiPhong : UserControl
	{
		private ObservableCollection<LoaiPhong> listLP;
		private CollectionView view;

		public uc_QuanLyLoaiPhong()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		#region Method

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(lsvLoaiPhong.ItemsSource).Refresh();
		}

		private void TaiDanhSach()
		{
			listLP = new ObservableCollection<LoaiPhong>(LoaiPhongBUS.GetInstance().getDataLoaiPhong());
			lsvLoaiPhong.ItemsSource = listLP;
			view = (CollectionView)CollectionViewSource.GetDefaultView(listLP);
			view.Filter = LoaiPhongFilter;
		}

		private bool LoaiPhongFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text))
				return true;
			return (obj as LoaiPhong).TenLoaiPhong.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private void nhanData(LoaiPhong loaiPhong)
		{
			if (!LoaiPhongBUS.GetInstance().KiemTraTrungTen(loaiPhong))
			{
				if (LoaiPhongBUS.GetInstance().addLoaiPhong(loaiPhong))
				{
					new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
			else
			{
				if (LoaiPhongBUS.GetInstance().hienThiLaiLoaiPhong(loaiPhong.TenLoaiPhong))
				{
					new DialogCustoms("Tên loại phòng đã tồn tại trong hệ thống\nĐã cập nhật lại danh sách",
						"Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
		}

		private void capNhatData(LoaiPhong loaiPhong)
		{
			if (LoaiPhongBUS.GetInstance().capNhatDataLoaiPhong(loaiPhong))
			{
				new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiDanhSach();
			}
			else
			{
				new DialogCustoms("Tên loại phòng đã tồn tại", "Thông báo", DialogCustoms.OK).Show();
			}
		}

		#endregion

		#region Event

		private void btnThemLoaiPhong_Click(object sender, RoutedEventArgs e)
		{
			var ThemLoaiPhong = new Them_SuaLoaiPhong();
			ThemLoaiPhong.truyenLoaiPhong = nhanData;
			ThemLoaiPhong.ShowDialog();
		}

		private void btnXoaLoaiPhong_Click(object sender, RoutedEventArgs e)
		{
			var phong = (sender as Button).DataContext as LoaiPhong;

			var thongbao = new DialogCustoms("Bạn có thật sự muốn loai phòng " + phong.TenLoaiPhong, "Thông báo",
				DialogCustoms.YesNo);

			if (thongbao.ShowDialog() == true)
			{
				new DialogCustoms("Xoá thành công", "Thông báo", DialogCustoms.OK).Show();
				LoaiPhongBUS.GetInstance().xoaLoaiPhong(phong);
				TaiDanhSach();
			}
		}

		private void btnSuaLoaiPhong_Click(object sender, RoutedEventArgs e)
		{
			var phong = (sender as Button).DataContext as LoaiPhong;

			var CapNhatLoaiPhong = new Them_SuaLoaiPhong(phong);
			CapNhatLoaiPhong.suaLoaiPhong = capNhatData;
			CapNhatLoaiPhong.ShowDialog();
		}

		#endregion
	}
}