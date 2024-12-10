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
	/// Interaction logic for uc_QuanLyTaiKhoan.xaml
	/// </summary>
	public partial class uc_QuanLyTaiKhoan : UserControl
	{
		ObservableCollection<TaiKhoan> list;
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
			if (String.IsNullOrEmpty(txtFilter.Text))
				return true;
			else
				return (obj as TaiKhoan).Username.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		void nhanData(TaiKhoan taiKhoan)
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

		void capNhatData(TaiKhoan taiKhoan)
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
			TaiKhoan taiKhoan = (sender as Button).DataContext as TaiKhoan;

			var thongbao = new DialogCustoms("Bạn có thật sự muốn xóa " + taiKhoan.Username, "Thông báo", DialogCustoms.YesNo);

			if (thongbao.ShowDialog() == true)
			{
				new DialogCustoms("Xoá thành công", "Thông báo", DialogCustoms.OK).Show();
				TaiKhoanBUS.GetInstance().xoaTaiKhoan(taiKhoan);
				TaiDanhSach();
			}
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			Them_SuaTaiKhoan ThemTaiKhoan = new Them_SuaTaiKhoan();
			ThemTaiKhoan.truyenTaiKhoan = new Them_SuaTaiKhoan.truyenData(nhanData);
			ThemTaiKhoan.ShowDialog();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			TaiKhoan taiKhoan = (sender as Button).DataContext as TaiKhoan;
			Them_SuaTaiKhoan CapNhatTaiKhoan = new Them_SuaTaiKhoan(true, taiKhoan);
			CapNhatTaiKhoan.suaTaiKhoan = new Them_SuaTaiKhoan.suaData(capNhatData);
            CapNhatTaiKhoan.ShowDialog();
		}
	}
}
