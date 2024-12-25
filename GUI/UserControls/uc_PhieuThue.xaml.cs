using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BUS;
using DAL.DTO;
using GUI.View;

namespace GUI.UserControls
{
	public partial class uc_PhieuThue : UserControl
	{
		private ObservableCollection<PhieuThue_Custom> lsPhieuThueCusToms;

		public uc_PhieuThue()
		{
			InitializeComponent();
			lsPhieuThueCusToms =
				new ObservableCollection<PhieuThue_Custom>(PhieuThueBUS.GetInstance().getDataPhieuThue());
			lvPhieuThue.ItemsSource = lsPhieuThueCusToms;
		}

		public uc_PhieuThue(int maNV) : this()
		{
			MaNV = maNV;
		}

		public int MaNV { get; set; }

		private void click_DatPhong(object sender, RoutedEventArgs e)
		{
			var dp = new DatPhong(MaNV);
			dp.luuPhieuDatPhong = nhanThongBaoLuuTuFormDatPhong;
			dp.ShowDialog();
		}

		private void nhanThongBaoLuuTuFormDatPhong()
		{
			lsPhieuThueCusToms =
				new ObservableCollection<PhieuThue_Custom>(PhieuThueBUS.GetInstance().getDataPhieuThue());
			lvPhieuThue.ItemsSource = lsPhieuThueCusToms;
		}

		private void click_ChiTiet(object sender, RoutedEventArgs e)
		{
			var ptct = (sender as Button).DataContext as PhieuThue_Custom;
			if (ptct != null)
			{
				var ctPT = new ChiTietPhieuThue(ptct);
				ctPT.ShowDialog();
			}
		}


		private bool filterTimKiem(object obj)
		{
			if (string.IsNullOrEmpty(txbTimKiem.Text))
				return true;
			var objTenKH = RemoveVietnameseTone((obj as PhieuThue_Custom).TenKH);
			var timkiem = RemoveVietnameseTone(txbTimKiem.Text);
			return objTenKH.Contains(timkiem);
		}


		private void txbTimKiem_TextChanged(object sender, TextChangedEventArgs e)
		{
			var viewPT = (CollectionView)CollectionViewSource.GetDefaultView(lvPhieuThue.ItemsSource);
			viewPT.Filter = filterTimKiem;
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

		private void click_Xoa(object sender, RoutedEventArgs e)
		{
			var phieuThue = (sender as Button).DataContext as PhieuThue_Custom;
			var error = string.Empty;
			var dlg = new DialogCustoms("Bạn có muốn xóa phiếu thuê " + phieuThue.MaPhieuThue, "Thông báo",
				DialogCustoms.YesNo);
			if (dlg.ShowDialog() == true)
			{
				if (PhieuThueBUS.GetInstance().xoaPhieuThueTheoMaPhieuThue(phieuThue.MaPhieuThue, out error))
				{
					new DialogCustoms("Xóa phiếu thuê thành công !", "Thông báo", DialogCustoms.OK).ShowDialog();
					lsPhieuThueCusToms.Remove(phieuThue);
				}
				else
				{
					new DialogCustoms("Xóa phiếu thuê thất bại !\n Lỗi: " + error, "Thông báo", DialogCustoms.OK)
						.ShowDialog();
				}
			}
		}
	}
}