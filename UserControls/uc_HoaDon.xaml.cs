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
	///     Interaction logic for uc_HoaDon.xaml
	/// </summary>
	public partial class uc_HoaDon : UserControl
	{
		private ObservableCollection<HoaDon> list;

		public uc_HoaDon()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			TaiDanhSach();
			Console.WriteLine("Load Hoa Don");
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<HoaDon>(HoaDonBUS.GetInstance().GetHoaDons());
			lsvHoaDon.ItemsSource = list;
		}

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			var view = (CollectionView)CollectionViewSource.GetDefaultView(lsvHoaDon.ItemsSource);
			view.Filter = HoaDonFilter;
			CollectionViewSource.GetDefaultView(lsvHoaDon.ItemsSource).Refresh();
		}

		private bool HoaDonFilter(object obj)
		{
			if (string.IsNullOrEmpty(txtFilter.Text)) return true;

			return (obj as HoaDon).MaHD == int.Parse(txtFilter.Text);
		}

		private bool HoaDonFilterTheoNgay(object obj)
		{
			if (string.IsNullOrEmpty(dtpChonNgay.Text))
				return true;
			return (obj as HoaDon).NgayLap.ToShortDateString().Equals(dtpChonNgay.Text);
		}

		private void dtpChonNgay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			var view = (CollectionView)CollectionViewSource.GetDefaultView(lsvHoaDon.ItemsSource);
			view.Filter = HoaDonFilterTheoNgay;
			CollectionViewSource.GetDefaultView(lsvHoaDon.ItemsSource).Refresh();
		}

		private void chiTiet_Click(object sender, RoutedEventArgs e)
		{
			var hoaDon = (sender as Button).DataContext as HoaDon;
			if (hoaDon != null)
			{
				var xuatHoaDon = new XuatHoaDon(hoaDon);
				xuatHoaDon.ShowDialog();
			}
		}
	}
}