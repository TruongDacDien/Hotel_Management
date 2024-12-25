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
	///     Interaction logic for uc_QuanLyKhachHang.xaml
	/// </summary>
	public partial class uc_QuanLyKhachHang : UserControl
	{
		private ObservableCollection<KhachHang> list;
		private CollectionView view;

		public uc_QuanLyKhachHang()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<KhachHang>(KhachHangBUS.GetInstance().GetKhachHangs());
			lsvKhachHang.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(list);
			view.Filter = KhachHangFilter;
		}

		private bool KhachHangFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text))
				return true;
			return (obj as KhachHang).TenKH.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private void btnThemKhachHang_Click(object sender, RoutedEventArgs e)
		{
			var nhapThongTinKhach = new Them_SuaKhachHang();
			nhapThongTinKhach.truyenKhachHang = nhanData;
			nhapThongTinKhach.ShowDialog();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			var khachHang = (sender as Button).DataContext as KhachHang;
			var CapNhatThongTinKhach = new Them_SuaKhachHang(khachHang);
			CapNhatThongTinKhach.suaKhachHang = capNhatData;
			CapNhatThongTinKhach.ShowDialog();
		}

		private void btnXoa_Click(object sender, RoutedEventArgs e)
		{
			var khachHang = (sender as Button).DataContext as KhachHang;
			var ThongBao = new DialogCustoms("Bạn có thật sự muỗn xóa " + khachHang.TenKH, "Thông báo",
				DialogCustoms.YesNo);

			if (ThongBao.ShowDialog() == true)
				if (KhachHangBUS.GetInstance().xoaDataKhachHang(khachHang))
				{
					new DialogCustoms("Xóa thành công", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
		}

		private void nhanData(KhachHang khachHang)
		{
			var error = string.Empty;
			if (KhachHangBUS.GetInstance().kiemTraTonTaiKhachHang(khachHang.CCCD) == -1)
			{
				if (KhachHangBUS.GetInstance().addKhachHang(khachHang, out error))
				{
					new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
			else
			{
				if (KhachHangBUS.GetInstance().hienThiLaiKhachHang(khachHang.CCCD))
				{
					new DialogCustoms("Khách hàng đã có trong hệ thống\nĐã cập nhật lại danh sách", "Thông báo",
						DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
		}

		private void capNhatData(KhachHang khachHang)
		{
			if (KhachHangBUS.GetInstance().capNhatDataKhachHang(khachHang))
			{
				new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiDanhSach();
			}
		}

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(lsvKhachHang.ItemsSource).Refresh();
		}
	}
}