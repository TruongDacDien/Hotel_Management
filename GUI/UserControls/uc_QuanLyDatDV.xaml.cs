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
		private const string TrangThaiDaDat = "Dịch vụ đã đặt";
		private const string TrangThaiDaHuyChoHoanTien = "Đã hủy - Chờ hoàn tiền";
		private const string HinhThucOnline = "Online";
		private const string HinhThucDirect = "Direct";

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

		private void click_ChucNang(object sender, RoutedEventArgs e)
		{
			var item = (sender as Button)?.DataContext as CT_SDDichVu;
			if (item == null) return;

			var trangThai = item.TrangThai ?? string.Empty;
			var hinhThuc = item.HinhThucThanhToan ?? string.Empty;
			var daThanhToan = item.DaThanhToan.HasValue && item.DaThanhToan.Value;

			var isThanhToan = !item.MaCTPT.HasValue &&
			                  trangThai == TrangThaiDaDat &&
			                  !daThanhToan &&
			                  string.Equals(hinhThuc, HinhThucDirect, StringComparison.OrdinalIgnoreCase);

			if (isThanhToan)
			{
				var xuatHoaDon = new XuatHoaDon(MaNV, item, true, false, true);
				xuatHoaDon.ShowDialog();
			}
			else
			{
				var capNhatTrangThaiSauIn = trangThai != TrangThaiDaHuyChoHoanTien;
				var xuatHoaDon = new XuatHoaDon(MaNV, item, false, true, capNhatTrangThaiSauIn);
				xuatHoaDon.ShowDialog();
			}

			view?.Refresh();
		}
	}
}
