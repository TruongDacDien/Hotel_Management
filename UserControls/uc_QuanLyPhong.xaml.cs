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
	///     Interaction logic for uc_QuanLyPhong.xaml
	/// </summary>
	public partial class uc_QuanLyPhong : UserControl
	{
		private ObservableCollection<Phong> list;
		private CollectionView view;

		public uc_QuanLyPhong()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		private bool PhongFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text))
				return true;
			return (obj as Phong).SoPhong.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<Phong>(PhongBUS.GetInstance().getDataPhong());
			lsvPhong.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(list);
			view.Filter = PhongFilter;
		}

		private void nhanData(Phong p)
		{
			if (PhongBUS.GetInstance().addDataPhong(p))
			{
				new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiDanhSach();
			}
			else
			{
				if (PhongBUS.GetInstance().hienThiLaiPhong(p.SoPhong))
				{
					new DialogCustoms("Số phòng đã tồn tại trong hệ thống\nĐã cập nhật lại danh sách", "Thông báo",
						DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
		}

		private void capNhatData(Phong p)
		{
			if (PhongBUS.GetInstance().capNhatDataPhong(p))
			{
				new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiDanhSach();
			}
		}

		private void btnThemPhong_Click(object sender, RoutedEventArgs e)
		{
			var NhapThemPhong = new Them_SuaPhong();
			NhapThemPhong.truyen = nhanData;
			NhapThemPhong.ShowDialog();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			var phong = (sender as Button).DataContext as Phong;

			var nhapThemPhong = new Them_SuaPhong(phong);

			nhapThemPhong.sua = capNhatData;
			nhapThemPhong.ShowDialog();
		}

		private void btnXoaPhong_Click(object sender, RoutedEventArgs e)
		{
			var phong = (sender as Button).DataContext as Phong;
			var thongbao = new DialogCustoms("Bạn có thật sự muốn xóa phòng " + phong.SoPhong, "Thông báo",
				DialogCustoms.YesNo);

			if (thongbao.ShowDialog() == true)
			{
				new DialogCustoms("Xoá thành công", "Thông báo", DialogCustoms.OK).Show();
				PhongBUS.GetInstance().xoaDataPhong(phong);
				TaiDanhSach();
			}
		}

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(lsvPhong.ItemsSource).Refresh();
		}
	}
}