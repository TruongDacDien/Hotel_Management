using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BUS;
using DAL.DTO;
using GUI.View;

namespace GUI.UserControls
{
	public partial class uc_NhanVien : UserControl
	{
		private ObservableCollection<NhanVien> list;
		private CollectionView view;

		public uc_NhanVien()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		private void TimKiem_TextChanged(object sender, TextChangedEventArgs e)
		{
			var viewNV = (CollectionView)CollectionViewSource.GetDefaultView(lvNhanVien.ItemsSource);
			viewNV.Filter = filterTimKiem;
		}

		#region method

		private void TaiDanhSach()
		{
			list = new ObservableCollection<NhanVien>(NhanVienBUS.GetInstance().getDataNhanVien());
			lvNhanVien.ItemsSource = list;
			view = (CollectionView)CollectionViewSource.GetDefaultView(list);
			view.Filter = filterTimKiem;
		}

		private void nhanData(NhanVien nv)
		{
			var error = string.Empty;
			if (NhanVienBUS.GetInstance().kiemTraTonTaiNhanVien(nv.CCCD) == -1)
			{
				if (NhanVienBUS.GetInstance().addNhanVien(nv))
				{
					new DialogCustoms("Thêm nhân viên thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
					TaiDanhSach();
				}
			}
			else
			{
				if (NhanVienBUS.GetInstance().hienThiLaiNhanVien(nv.CCCD))
				{
					new DialogCustoms("Nhân viên đã có trong hệ thống\nĐã cập nhật lại danh sách", "Thông báo",
						DialogCustoms.OK).Show();
					TaiDanhSach();
				}
			}
		}

		private void SuaThongTinNhanVien(NhanVien nv)
		{
			// sửa để update lên list view
			var nhanVien_Sua = list.Where(s => s.MaNV.Equals(nv.MaNV)).FirstOrDefault();
			nhanVien_Sua.HoTen = nv.HoTen;
			nhanVien_Sua.GioiTinh = nv.GioiTinh;
			nhanVien_Sua.NTNS = nv.NTNS;
			nhanVien_Sua.Luong = nv.Luong;
			nhanVien_Sua.SDT = nv.SDT;
			nhanVien_Sua.CCCD = nv.CCCD;
			nhanVien_Sua.ChucVu = nv.ChucVu;
			nhanVien_Sua.DiaChi = nv.DiaChi;

			if (NhanVienBUS.GetInstance().updateNhanVien(nv))
			{
				new DialogCustoms("Cập nhật nhân viên thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
				TaiDanhSach();
			}
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

		private bool filterTimKiem(object obj)
		{
			if (string.IsNullOrEmpty(txbTimKiem.Text))
				return true;
			var objTenNV = RemoveVietnameseTone((obj as NhanVien).HoTen);
			var timkiem = RemoveVietnameseTone(txbTimKiem.Text);
			return objTenNV.Contains(timkiem);
		}

		#endregion

		#region event

		private void click_ThemNV(object sender, RoutedEventArgs e)
		{
			var tnv = new Them_SuaNhanVien();
			tnv.themNhanVien = nhanData;
			tnv.ShowDialog();
		}


		private void click_XoaNV(object sender, RoutedEventArgs e)
		{
			var nv = (sender as Button).DataContext as NhanVien;
			var dlg = new DialogCustoms("Bạn có muốn xóa nhân viên " + nv.HoTen, "Thông báo", DialogCustoms.YesNo);
			if (dlg.ShowDialog() == true)
				if (NhanVienBUS.GetInstance().deleteNhanVien(nv))
				{
					new DialogCustoms("Xóa nhân viên thành công !", "Thông báo", DialogCustoms.OK).ShowDialog();
					TaiDanhSach();
				}
		}

		private void click_SuaNV(object sender, RoutedEventArgs e)
		{
			var nv = (sender as Button).DataContext as NhanVien;
			var themNhanVien = new Them_SuaNhanVien();
			themNhanVien.truyenNhanVien(nv);
			themNhanVien.suaNhanVien = SuaThongTinNhanVien;
			themNhanVien.ShowDialog();
		}

		#endregion
	}
}