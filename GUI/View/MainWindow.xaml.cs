﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BUS;
using DAL.DTO;
using GUI.UserControls;
using Microsoft.Win32;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private readonly TaiKhoanNV taiKhoan;
		private readonly PhanQuyen phanQuyen;

		public MainWindow()
		{
			InitializeComponent();
		}

		public MainWindow(TaiKhoanNV tk) : this()
		{
			taiKhoan = tk;
			MaNV = tk.MaNV;
			phanQuyen = TaiKhoanBUS.GetInstance().layPhanQuyenTaiKhoan(tk.MaTKNV);
			MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
			MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
		}

		#region uc_view

		private uc_Home Home;
		private uc_Phong Phong_UC;
		private uc_PhieuThue ThuePhong_UC;
		private uc_NhanVien NhanVien_UC;
		private uc_QuanLyPhong QuanLyPhong_UC;
		private uc_QuanLyKhachHang QuanLyKhachHang_UC;
		private uc_QuanLyLoaiPhong QuanLyLoaiPhong_UC;
		private uc_QuanLyDichVu QuanLyDichVu_UC;
		private uc_QuanLyTienNghi QuanLyTienNghi_UC;
		//private uc_QuanLyChiTietTienNghi QuanLyChiTietTienNghi_UC;
		private uc_QuanLyLoaiDichVu QuanLyLoaiDichVu_UC;
		private uc_QuanLyTaiKhoan QuanLyTaiKhoan_UC;
		private uc_HoaDon HoaDon_UC;
		private uc_ThongKe ThongKe_UC;
		private uc_DDXQ DDXQ_UC;

        #endregion

        #region Khai báo biến

        public List<ItemMenuMainWindow> listMenu { get; set; }
		private string title_Main;
		private int minHeight_ucControlbar;

		public class ItemMenuMainWindow
		{
			public string name { get; set; }
			public string foreColor { get; set; }
			public string kind_Icon { get; set; }
		}

		public string Title_Main
		{
			get => title_Main;
			set
			{
				title_Main = value;
				OnPropertyChanged("Title_Main");
			}
		}

		public int MinHeight_ucControlbar
		{
			get => minHeight_ucControlbar;
			set
			{
				minHeight_ucControlbar = value;
				OnPropertyChanged("MinHeight_ucControlbar");
				if (value == 1)
					boGoc.Rect = new Rect(0, 0, SystemParameters.MaximizedPrimaryScreenWidth,
						SystemParameters.MaximizedPrimaryScreenHeight);
				else
					boGoc.Rect = new Rect(0, 0, 1300, 700);
			}
		}

		public int MaNV { get; set; }

		#endregion

		#region method

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string newName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(newName));
		}

		private void initListViewMenu()
		{
			listMenu = new List<ItemMenuMainWindow>();
			if (phanQuyen == null)
			{
				new DialogCustoms("Không thể lấy phân quyền!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			//Khoi tao Menu
			if (phanQuyen.TrangChu)
				listMenu.Add(new ItemMenuMainWindow { name = "Trang Chủ", foreColor = "Gray", kind_Icon = "Home" });

			if (phanQuyen.Phong)
				listMenu.Add(new ItemMenuMainWindow { name = "Phòng", foreColor = "#FFF08033", kind_Icon = "HomeCity" });

			if (phanQuyen.DatPhong)
				listMenu.Add(new ItemMenuMainWindow { name = "Đặt Phòng", foreColor = "Green", kind_Icon = "BookAccount" });

			if (phanQuyen.HoaDon)
				listMenu.Add(new ItemMenuMainWindow { name = "Hóa đơn", foreColor = "#FFD41515", kind_Icon = "Receipt" });

			if (phanQuyen.QLKhachHang)
				listMenu.Add(new ItemMenuMainWindow { name = "QL khách hàng", foreColor = "#FFD41515", kind_Icon = "AccountGroup" });

			if (phanQuyen.QLPhong)
				listMenu.Add(new ItemMenuMainWindow { name = "QL phòng", foreColor = "#FFE6A701", kind_Icon = "StarCircle" });

			if (phanQuyen.QLLoaiPhong)
				listMenu.Add(new ItemMenuMainWindow { name = "QL loại phòng", foreColor = "#FFE6A701", kind_Icon = "StarCircle" });

			if (phanQuyen.QLDichVu)
				listMenu.Add(new ItemMenuMainWindow { name = "QL dịch vụ", foreColor = "Blue", kind_Icon = "FaceAgent" });

			if (phanQuyen.QLLoaiDichVu)
				listMenu.Add(new ItemMenuMainWindow { name = "QL loại dịch vụ", foreColor = "Blue", kind_Icon = "FaceAgent" });

			if (phanQuyen.QLTienNghi)
				listMenu.Add(new ItemMenuMainWindow { name = "QL tiện nghi", foreColor = "#FFF08033", kind_Icon = "Fridge" });

			// Nếu có thêm QLChiTietTienNghi, bạn thêm ở đây

			if (phanQuyen.QLNhanVien)
				listMenu.Add(new ItemMenuMainWindow { name = "QL nhân Viên", foreColor = "#FFD41515", kind_Icon = "AccountHardHat" });

			if (phanQuyen.QLTaiKhoan)
				listMenu.Add(new ItemMenuMainWindow { name = "QL tài khoản", foreColor = "Blue", kind_Icon = "AccountCog" });

			if (phanQuyen.ThongKe)
				listMenu.Add(new ItemMenuMainWindow { name = "Thống kê", foreColor = "#FF0069C1", kind_Icon = "ChartAreaspline" });

            if (phanQuyen.QLDDXQ)
                listMenu.Add(new ItemMenuMainWindow { name = "QL DĐXQ", foreColor = "#FF0069C1", kind_Icon = "MapMarker" });

            // Tùy chọn thêm nếu bạn có các chức năng thông báo và lịch sử hoạt động
            if (phanQuyen.ThongBao)
				listMenu.Add(new ItemMenuMainWindow { name = "Thông báo", foreColor = "Orange", kind_Icon = "Bell" });

			if (phanQuyen.LichSuHoatDong)
				listMenu.Add(new ItemMenuMainWindow { name = "Lịch sử hoạt động", foreColor = "Gray", kind_Icon = "History" });

			lisviewMenu.ItemsSource = listMenu;
			lisviewMenu.SelectedValuePath = "name";
			Title_Main = "Trang Chủ";
		}

		#endregion

		#region event

		private void load_Windows(object sender, RoutedEventArgs e)
		{
			DataContext = this;
			Home = new uc_Home();
			contenDisplayMain.Content = Home;
			txbHoTenNV.Text = taiKhoan.NhanVien.HoTen;
			LoadAvatar();
			initListViewMenu();
		}

		private void LoadAvatar()
		{
			try
			{
				var bitmap = new BitmapImage();

				//Bust cache bằng cách thêm chuỗi ngẫu nhiên vào URL
				var uri = new Uri($"{taiKhoan.AvatarURL}?v={Guid.NewGuid()}", UriKind.Absolute);

				bitmap.BeginInit();
				bitmap.UriSource = uri;
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache; // Cũng giúp không dùng cache
				bitmap.EndInit();

				imgAvatar.Fill = new ImageBrush(bitmap);
			}
			catch (Exception ex)
			{
				new DialogCustoms($"Không thể tải ảnh đại diện: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
			}
		}

		private void lisviewMenu_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (lisviewMenu.SelectedValue != null)
			{
				switch (lisviewMenu.SelectedIndex)
				{
					case 0:
						//Đang là Home rồi thì không set nữa
						if (Title_Main.Equals(lisviewMenu.SelectedValue.ToString())) break;
						contenDisplayMain.Content = Home;
						break;
					case 1:
						Phong_UC = new uc_Phong(MaNV);
						contenDisplayMain.Content = Phong_UC;
						break;
					case 2:
						ThuePhong_UC = new uc_PhieuThue(MaNV);
						contenDisplayMain.Content = ThuePhong_UC;
						break;
					case 3:
						HoaDon_UC = new uc_HoaDon();
						contenDisplayMain.Content = HoaDon_UC;
						break;
					case 4:
						QuanLyKhachHang_UC = new uc_QuanLyKhachHang();
						contenDisplayMain.Content = QuanLyKhachHang_UC;
						break;
					case 5:
						QuanLyPhong_UC = new uc_QuanLyPhong();
						contenDisplayMain.Content = QuanLyPhong_UC;
						break;
					case 6:
						QuanLyLoaiPhong_UC = new uc_QuanLyLoaiPhong();
						contenDisplayMain.Content = QuanLyLoaiPhong_UC;
						break;
					case 7:
						QuanLyDichVu_UC = new uc_QuanLyDichVu();
						contenDisplayMain.Content = QuanLyDichVu_UC;
						break;
					case 8:
						QuanLyLoaiDichVu_UC = new uc_QuanLyLoaiDichVu();
						contenDisplayMain.Content = QuanLyLoaiDichVu_UC;
						break;
					case 9:
						QuanLyTienNghi_UC = new uc_QuanLyTienNghi();
						contenDisplayMain.Content = QuanLyTienNghi_UC;
						break;
					//case 10:
					//	QuanLyChiTietTienNghi_UC = new uc_QuanLyChiTietTienNghi();
					//	contenDisplayMain.Content = QuanLyChiTietTienNghi_UC;
					//	break;
					case 10:
						NhanVien_UC = new uc_NhanVien();
						contenDisplayMain.Content = NhanVien_UC;
						break;
					case 11:
						QuanLyTaiKhoan_UC = new uc_QuanLyTaiKhoan();
						contenDisplayMain.Content = QuanLyTaiKhoan_UC;
						break;
					case 12:
						ThongKe_UC = new uc_ThongKe();
						contenDisplayMain.Content = ThongKe_UC;
						break;
                    case 13:
                        DDXQ_UC = new uc_DDXQ();
                        contenDisplayMain.Content = DDXQ_UC;
                        break;
                }

				Title_Main = lisviewMenu.SelectedValue.ToString();
				//Tự động hóa việc click Button toggleBtnMenu_Close
				btnCloseLVMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
			}
		}

		private void ListBoxItem_Click(object sender, MouseButtonEventArgs e)
		{
			ListBoxItem clickedItem = sender as ListBoxItem;
			if (clickedItem != null)
			{
				string itemContent = clickedItem.Content.ToString();

				switch (itemContent)
				{
					case "Thông tin cá nhân":
						MW_ThongTinCaNhan thongTinCaNhan = new MW_ThongTinCaNhan(MaNV);
						thongTinCaNhan.ShowDialog();
						break;
					case "Thông tin tài khoản":
						MW_ThongTinTaiKhoan thongTinTaiKhoan = new MW_ThongTinTaiKhoan(taiKhoan);
						thongTinTaiKhoan.ShowDialog();
						LoadAvatar();
						break;
					case "Đăng xuất":
						var dialog = new DialogCustoms("Bạn có muốn đăng xuất ?", "Thông báo", DialogCustoms.YesNo);
						if (dialog.ShowDialog() == true)
						{
							new DangNhap().Show();
							Close();
						}
						break;
					default:
						break;
				}
			}
		}

		#endregion
	}
}