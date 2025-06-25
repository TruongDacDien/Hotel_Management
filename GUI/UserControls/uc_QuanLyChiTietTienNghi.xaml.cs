//using System;
//using System.Collections.ObjectModel;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using BUS;
//using DAL.DTO;
//using GUI.View;

//namespace GUI.UserControls
//{
//	/// <summary>
//	///     Interaction logic for uc_QuanLyChiTietTienNghi.xaml
//	/// </summary>
//	public partial class uc_QuanLyChiTietTienNghi : UserControl
//	{
//		private ObservableCollection<CT_TienNghi> list;
//		private CollectionView view;

//		public uc_QuanLyChiTietTienNghi()
//		{
//			InitializeComponent();
//			TaiDanhSach();
//		}

//		private void TaiDanhSach()
//		{
//			list = new ObservableCollection<CT_TienNghi>(CT_TienNghiBUS.GetInstance().getData());
//			lsvCTTienNghi.ItemsSource = list;
//			view = (CollectionView)CollectionViewSource.GetDefaultView(lsvCTTienNghi.ItemsSource);
//			view.Filter = CTTienNghiFilter;
//		}


//		private bool CTTienNghiFilter(object obj)
//		{
//			if (string.IsNullOrEmpty(txtFilter.Text))
//				return true;
//			return (obj as CT_TienNghi).MaLoaiPhong.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
//		}

//		private void btnThemCTTienNghi_Click(object sender, RoutedEventArgs e)
//		{
//			var ThemCTTienNghi = new Them_SuaChiTietTienNghi(false);
//			ThemCTTienNghi.truyenCT = nhanData;
//			ThemCTTienNghi.ShowDialog();
//		}

//		private void btnSuaCTTienNghi_Click(object sender, RoutedEventArgs e)
//		{
//			var ctTienNghi = (sender as Button).DataContext as CT_TienNghi;
//			var CapNhatCTTienNghi = new Them_SuaChiTietTienNghi(true, ctTienNghi);
//			CapNhatCTTienNghi.suaCT = capNhatData;
//			CapNhatCTTienNghi.ShowDialog();
//		}

//		private void btnXoaCTTienNghi_Click(object sender, RoutedEventArgs e)
//		{
//			var cT_TienNghi = (sender as Button).DataContext as CT_TienNghi;

//			var ThongBao =
//				new DialogCustoms(
//					"Bạn có thật sự muốn xóa tiện nghi " + cT_TienNghi.TenTN + " ở phòng " + cT_TienNghi.SoPhong,
//					"Thông báo", DialogCustoms.YesNo);

//			if (ThongBao.ShowDialog() == true)
//			{
//				new DialogCustoms("Xóa thành công thành công", "Thông báo", DialogCustoms.OK).ShowDialog();
//				CT_TienNghiBUS.GetInstance().xoaCTTienNghi(cT_TienNghi);
//				TaiDanhSach();
//			}
//		}

//		private void nhanData(CT_TienNghi cT_TienNghi)
//		{
//			if (!CT_TienNghiBUS.GetInstance().KiemTraTonTai(cT_TienNghi))
//			{
//				if (CT_TienNghiBUS.GetInstance().addCTTienNghi(cT_TienNghi))
//				{
//					new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
//					TaiDanhSach();
//				}
//			}
//			else
//			{
//				if (CT_TienNghiBUS.GetInstance().hienThiLaiCT_TienNghi(cT_TienNghi.MaTN, cT_TienNghi.MaLoaiPhong))
//				{
//					new DialogCustoms("Tiện nghi đã tồn tại trong hệ thống\nĐã cập nhật lại danh sách", "Thông báo",
//						DialogCustoms.OK).Show();
//					TaiDanhSach();
//				}
//			}
//		}

//		private void capNhatData(CT_TienNghi cT_TienNghi)
//		{
//			if (CT_TienNghiBUS.GetInstance().capNhatCTTienNghi(cT_TienNghi))
//			{
//				new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
//				TaiDanhSach();
//			}

//			else
//			{
//				new DialogCustoms("Tiện nghi đã tồn tại", "Thông báo", DialogCustoms.OK).Show();
//			}
//		}

//		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
//		{
//			CollectionViewSource.GetDefaultView(lsvCTTienNghi.ItemsSource).Refresh();
//		}
//	}
//}