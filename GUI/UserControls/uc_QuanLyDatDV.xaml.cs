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
	/// Interaction logic for uc_QuanLyDatDV.xaml
	/// </summary>
	public partial class uc_QuanLyDatDV : UserControl
	{
		private ObservableCollection<CT_SDDichVu> list;
		private CollectionView view;

		public uc_QuanLyDatDV(int maNV) : this()
		{
			MaNV = maNV;
		}

		private uc_QuanLyDatDV()
		{
			InitializeComponent();
		}

		public int MaNV { get; }

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			TaiDanhSach();
		}

		private void TaiDanhSach()
		{
			list = new ObservableCollection<CT_SDDichVu>(CTSDDV_BUS.GetInstance().getCTSDDVQuanLy());
			lsvCTSDDV.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(list);
			view.Filter = FilterCTSDDV;
		}

		private bool FilterCTSDDV(object obj)
		{
			if (string.IsNullOrWhiteSpace(txtFilter.Text))
				return true;

			var item = obj as CT_SDDichVu;
			if (item == null) return false;

			var keyword = txtFilter.Text.Trim();
			return (item.TenKH != null && item.TenKH.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
			       (item.TenDV != null && item.TenDV.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
			       (item.SoPhong != null && item.SoPhong.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
		}

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			view?.Refresh();
		}

		private void click_SuDung(object sender, RoutedEventArgs e)
		{
			var item = (sender as Button)?.DataContext as CT_SDDichVu;
			if (item == null) return;

			var error = string.Empty;
			var trangThaiDaSuDung = "Đã sử dụng";
			if (!CTSDDV_BUS.GetInstance().capNhatTrangThaiSuDung(item.MaCTSDDV, trangThaiDaSuDung, true, "Direct",
				    out error))
			{
				new DialogCustoms("Loi: " + error, "Thong bao", DialogCustoms.OK).ShowDialog();
				return;
			}

			item.TrangThai = trangThaiDaSuDung;
			item.DaThanhToan = true;

			var hd = new HoaDon
			{
				MaNV = MaNV,
				MaCTPT = item.MaCTPT,
				MaCTSDDV = item.MaCTSDDV,
				NgayLap = DateTime.Now,
				TongTien = item.ThanhTien
			};

			if (!HoaDonBUS.GetInstance().themHoaDon(hd, out error))
			{
				new DialogCustoms("Loi tao hoa don: " + error, "Thong bao", DialogCustoms.OK).ShowDialog();
			}

			view?.Refresh();
		}

		private void click_InPhieu(object sender, RoutedEventArgs e)
		{
			var item = (sender as Button)?.DataContext as CT_SDDichVu;
			if (item == null) return;

			var inPhieu = new XuatPhieuDichVu(item);
			inPhieu.ShowDialog();
		}
	}
}
