using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BUS;
using DAL.DTO;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for CTP_ThemDV.xaml
	/// </summary>
	public partial class CTP_ThemDV : Window
	{
		public delegate void Delegate_CTPDV(ObservableCollection<DichVu_DaChon> obDVCT);

		private readonly int? maCTPhieuThue;
		private List<DichVu> lsCache;

		private ObservableCollection<DichVu> lsdichVu_Customs;
		private ObservableCollection<DichVu_DaChon> lsDichVu_DaChon;
		private List<LoaiDV> lsLoaiDV;
		public Delegate_CTPDV truyenData;

		public CTP_ThemDV()
		{
			InitializeComponent();
		}

		public CTP_ThemDV(int? mactpt) : this()
		{
			maCTPhieuThue = mactpt;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			lsdichVu_Customs = new ObservableCollection<DichVu>(DichVuBUS.GetInstance().getDichVu_Custom());
			lsDichVu_DaChon = new ObservableCollection<DichVu_DaChon>();
			lsCache = new List<DichVu>();
			lsLoaiDV = new List<LoaiDV>();
			lsLoaiDV = LoaiDichVuBUS.GetInstance().getDataLoaiDV();
			lsLoaiDV.Insert(0, new LoaiDV { TenLoaiDV = "Tất cả", MaLoaiDV = 0 });
			cbTimKiemLoaiDV.SelectedIndex = 0;
			lvDanhSachDV.ItemsSource = lsdichVu_Customs;
			lvDichVuDaChon.ItemsSource = lsDichVu_DaChon;
			cbTimKiemLoaiDV.ItemsSource = lsLoaiDV;
			cbTimKiemLoaiDV.DisplayMemberPath = "TenLoaiDV";
			cbTimKiemLoaiDV.SelectedValuePath = "MaLoaiDV";
		}

		private void click_Them(object sender, RoutedEventArgs e)
		{
			var dvct = (sender as Button).DataContext as DichVu;
			lsDichVu_DaChon.Add(new DichVu_DaChon
				{ ThanhTien = dvct.Gia, TenDV = dvct.TenDV, SoLuong = 1, MaDV = dvct.MaDV, Gia = dvct.Gia });
			lsCache.Add(dvct);
			lsdichVu_Customs.Remove(dvct);
		}

		private void click_Xoa(object sender, RoutedEventArgs e)
		{
			var dvdachon = (sender as Button).DataContext as DichVu_DaChon;
			var dichVu_Custom = lsCache.Where(p => p.MaDV.Equals(dvdachon.MaDV)).FirstOrDefault();
			lsdichVu_Customs.Add(dichVu_Custom);
			lsDichVu_DaChon.Remove(dvdachon);
		}

		private void click_Thoat(object sender, RoutedEventArgs e)
		{
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void click_Luu(object sender, RoutedEventArgs e)
		{
			var error = string.Empty;
			var dem = 0;
			foreach (var item in lsDichVu_DaChon)
			{
				var ct = new CT_SDDichVu
				{
					MaCTPT = maCTPhieuThue,
					MaDV = item.MaDV,
					SL = item.SoLuong == null ? 0 : int.Parse(item.SoLuong.ToString()),
					ThanhTien = item.ThanhTien == null ? 0 : decimal.Parse(item.ThanhTien.ToString())
				};

				if (CTSDDV_BUS.GetInstance().addDataCTSDDC(ct, out error))
					dem++;
				else
					new DialogCustoms("Lỗi: " + error, "Thông báo", DialogCustoms.OK).ShowDialog();
			}

			if (dem == lsDichVu_DaChon.Count)
			{
				new DialogCustoms("Thêm dịch vụ sử dụng thành công !", "Thông báo", DialogCustoms.OK).ShowDialog();
				if (truyenData != null) truyenData(lsDichVu_DaChon);
			}

			Close();
		}

		private void txbSoLuong_LostFocus(object sender, RoutedEventArgs e)
		{
			var txb = sender as TextBox;
			var dvdc = (sender as TextBox).DataContext as DichVu_DaChon;
			var soLuong = 1;
			if (!int.TryParse(txb.Text, out soLuong))
			{
				new DialogCustoms("Lỗi: Nhập số lượng kiểu số nguyên!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			dvdc.SoLuong = soLuong;
			dvdc.ThanhTien = dvdc.Gia * soLuong;
		}

		private void txbSoLuong_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var txb = sender as TextBox;
				var dvdc = (sender as TextBox).DataContext as DichVu_DaChon;
				var soLuong = 1;
				if (!int.TryParse(txb.Text, out soLuong))
				{
					new DialogCustoms("Lỗi: Nhập số lượng kiểu số nguyên!", "Thông báo", DialogCustoms.OK).ShowDialog();
					return;
				}

				dvdc.SoLuong = soLuong;
				dvdc.ThanhTien = dvdc.Gia * soLuong;
			}
		}

		private void txbTimKiem_TextChanged(object sender, TextChangedEventArgs e)
		{
			var viewDV = (CollectionView)CollectionViewSource.GetDefaultView(lvDanhSachDV.ItemsSource);
			viewDV.Filter = filterTimKiem;
		}

		private void cbTimKiemLoaiDV_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var viewDV = (CollectionView)CollectionViewSource.GetDefaultView(lvDanhSachDV.ItemsSource);
			viewDV.Filter = filterTimKiemLoaiDV;
		}

		#region method

		private bool filterTimKiem(object obj)
		{
			if (string.IsNullOrEmpty(txbTimKiem.Text))
				return true;
			var objTenDV = RemoveVietnameseTone((obj as DichVu).TenDV);
			var timkiem = RemoveVietnameseTone(txbTimKiem.Text);
			return objTenDV.Contains(timkiem);
		}

		private bool filterTimKiemLoaiDV(object obj)
		{
			var dichVu = obj as DichVu;
			var selectedLoaiDV = cbTimKiemLoaiDV.SelectedItem as LoaiDV;
			if (selectedLoaiDV == null ||
			    selectedLoaiDV.TenLoaiDV == "Tất cả") // Nếu không chọn loại dịch vụ hoặc chọn "Tất cả", hiển thị tất cả
				return true;
			return
				dichVu.MaLoaiDV ==
				selectedLoaiDV.MaLoaiDV; // So sánh MaLoaiDV của DichVu với MaLoaiDV của loại dịch vụ được chọn
		}

		public string RemoveVietnameseTone(string text)
		{
			var result = text.ToLower();
			result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
			result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
			result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
			result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
			result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
			result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
			result = Regex.Replace(result, "đ", "d");
			return result;
		}

		#endregion
	}
}