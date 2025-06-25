using BUS;
using DAL.DTO;
using GUI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.UserControls
{
	/// <summary>
	/// Interaction logic for uc_QuanLyTaiKhoanKH.xaml
	/// </summary>
	public partial class uc_QuanLyTaiKhoanKH : UserControl
	{
		private ObservableCollection<TaiKhoanKH> list;
		private CollectionView view;

		public uc_QuanLyTaiKhoanKH()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<TaiKhoanKH>(TaiKhoanKHBUS.GetInstance().getDataTaiKhoan());
			lsvTaiKhoan.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(list);
			view.Filter = TaiKhoanFilter;
		}

		private bool TaiKhoanFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text))
				return true;
			return (obj as TaiKhoanKH).Username.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private bool nhanData(TaiKhoanKH taiKhoan)
		{
			if (!TaiKhoanKHBUS.GetInstance().kiemTraTrungUsername(taiKhoan.Username))
			{
				if (TaiKhoanKHBUS.GetInstance().themTaiKhoan(taiKhoan))
				{
					new DialogCustoms("Thêm tài khoản thành công", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
					return true;
				}
			}
			else
			{
				if (TaiKhoanKHBUS.GetInstance().hienThiLaiTaiKhoan(taiKhoan.Username))
				{
					new DialogCustoms("Tên tài khoản đã tồn tại trong hệ thống", "Thông báo", DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
			return false;
		}

		private void capNhatData(TaiKhoanKH taiKhoan)
		{
			if (TaiKhoanKHBUS.GetInstance().capNhatTaiKhoan(taiKhoan))
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
			var taiKhoan = (sender as Button).DataContext as TaiKhoanKH;

			var thongbao = new DialogCustoms("Bạn có thật sự muốn xóa " + taiKhoan.Username, "Thông báo",
				DialogCustoms.YesNo);

			if (thongbao.ShowDialog() == true)
			{
				new DialogCustoms("Xoá thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiKhoanKHBUS.GetInstance().xoaTaiKhoan(taiKhoan);
				TaiDanhSach();
			}
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			var ThemTaiKhoan = new Them_SuaTaiKhoanKH();
			ThemTaiKhoan.truyenTaiKhoan = nhanData;
			ThemTaiKhoan.ShowDialog();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			var taiKhoan = (sender as Button).DataContext as TaiKhoanKH;
			var CapNhatTaiKhoan = new Them_SuaTaiKhoanKH(true, taiKhoan);
			CapNhatTaiKhoan.suaTaiKhoan = capNhatData;
			CapNhatTaiKhoan.ShowDialog();
		}
	}
}
