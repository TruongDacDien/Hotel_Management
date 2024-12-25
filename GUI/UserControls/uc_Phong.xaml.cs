using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using BUS;
using DAL.DTO;
using GUI.View;

namespace GUI.UserControls
{
	/// <summary>
	///     Interaction logic for uc_Phong.xaml
	/// </summary>
	public partial class uc_Phong : UserControl
	{
		private uc_Phong()
		{
			InitializeComponent();
			lsTrong = new ObservableCollection<Phong_Custom>();
			lsPhong_Custom = PhongBUS.GetInstance().getDataPhong_Custom();
			initEvent();
		}

		public uc_Phong(int maNV) : this()
		{
			MaNV = maNV;
		}

		#region Khai báo biến

		private ObservableCollection<Phong_Custom> lsPhong_Custom;
		private ObservableCollection<Phong_Custom> lsTrong;
		public int MaNV { get; set; }

		private string lastTinhTrangSelection;
		private string lastDonDepSelection;
		private string lastLoaiPhongSelection;

		#endregion

		#region Method

		private void ThemRadioButtonLoaiPhong(string loaiPhong)
		{
			// Kiểm tra xem radio button đã tồn tại chưa
			if (spLoaiPhong.Children.OfType<RadioButton>().Any(rb => rb.Content.ToString() == loaiPhong)) return;
			// Tạo mới radio button
			var newRadioButton = new RadioButton
			{
				Content = loaiPhong,
				GroupName = "LoaiPhong",
				Margin = new Thickness(3),
				FontSize = 15,
				Height = 24
			};
			newRadioButton.Click += rb_Click; // Gắn sự kiện click
			spLoaiPhong.Children.Add(newRadioButton);
		}

		private void ThemDanhSachPhong(string loaiPhong)
		{
			// Tạo tiêu đề cho phòng
			var title = new TextBlock
			{
				Text = loaiPhong,
				FontSize = 20,
				Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00A3FF")),
				Margin = new Thickness(40, 20, 0, 0)
			};

			// Lọc danh sách phòng theo loại phòng
			var filteredPhong = lsPhong_Custom.Where(p => p.LoaiPhong == loaiPhong).ToList();

			// Tạo ListView cho phòng
			var listView = new ListView
			{
				Margin = new Thickness(10, 10, 10, 10),
				ItemsSource = new ObservableCollection<Phong_Custom>(filteredPhong),
				ItemTemplate = (DataTemplate)Resources["PhongItemTemplate"],
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Width = spDanhSachPhong.ActualWidth,
				Name = $"lv{loaiPhong.Replace(" ", "")}"
			};

			// Sử dụng WrapPanel cho kiểu ma trận
			var itemsPanelTemplate = new ItemsPanelTemplate();
			var factory = new FrameworkElementFactory(typeof(WrapPanel));
			factory.SetValue(WrapPanel.OrientationProperty, Orientation.Horizontal);
			factory.SetValue(MaxWidthProperty, spDanhSachPhong.ActualWidth);
			itemsPanelTemplate.VisualTree = factory;
			listView.ItemsPanel = itemsPanelTemplate;

			// Khởi tạo sự kiện cho ListView
			listView.PreviewMouseLeftButtonUp += ListViewPhong_PreviewMouseLeftButtonUp;

			// Thêm tiêu đề và ListView vào StackPanel
			spDanhSachPhong.Children.Add(title);
			spDanhSachPhong.Children.Add(listView);
		}

		private void refeshLoaiPhong()
		{
			spDanhSachPhong.Children.Clear(); // Xóa danh sách hiện tại
			spLoaiPhong.Children.Clear(); // Xóa các radio button cũ

			// Kiểm tra dữ liệu
			if (lsPhong_Custom == null || !lsPhong_Custom.Any())
			{
				new DialogCustoms("Không có dữ liệu phòng để hiển thị!", "Thông báo", DialogCustoms.OK).Show();
				return;
			}

			// Thêm radio button "Tất cả loại phòng"
			var rbTatCaLoaiPhong = new RadioButton
			{
				Content = "Tất cả loại phòng",
				GroupName = "LoaiPhong",
				Margin = new Thickness(3),
				FontSize = 15,
				Height = 24,
				IsChecked = true // Mặc định chọn
			};
			rbTatCaLoaiPhong.Click += rb_Click;
			spLoaiPhong.Children.Add(rbTatCaLoaiPhong);

			// Thêm các loại phòng từ danh sách
			var allLoaiPhong = lsPhong_Custom.Select(p => p.LoaiPhong).Distinct().ToList();

			foreach (var loaiPhong in allLoaiPhong)
			{
				ThemRadioButtonLoaiPhong(loaiPhong); // Tạo các radio button cho loại phòng
				ThemDanhSachPhong(loaiPhong); // Tạo danh sách phòng tương ứng
			}
		}

		private void initEvent()
		{
			// Gắn sự kiện Click cho RadioButton trong spTrangThai
			foreach (var radioButton in spTrangThai.Children.OfType<RadioButton>()) radioButton.Click += rb_Click;

			// Gắn sự kiện Click cho RadioButton trong spDonDep
			foreach (var radioButton in spDonDep.Children.OfType<RadioButton>()) radioButton.Click += rb_Click;
		}

		private bool PhongFilter(object obj)
		{
			if (!(obj is Phong_Custom ph)) return false;

			// Lấy các RadioButton được chọn
			var radioTinhTrang = spTrangThai.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
			var radioDonDep = spDonDep.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
			var radioLoaiPhong = spLoaiPhong.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);

			// Điều kiện lọc theo Tình Trạng
			var matchTinhTrang = radioTinhTrang == null || radioTinhTrang.Content.ToString() == "Tất cả phòng" ||
			                     ph.TinhTrang == radioTinhTrang.Content.ToString();

			// Điều kiện lọc theo Dọn Dẹp
			var matchDonDep = radioDonDep == null || radioDonDep.Content.ToString() == "Tất cả" ||
			                  ph.DonDep == radioDonDep.Content.ToString();

			// Điều kiện lọc theo Loại Phòng
			var matchLoaiPhong = radioLoaiPhong == null || radioLoaiPhong.Content.ToString() == "Tất cả loại phòng" ||
			                     ph.LoaiPhong == radioLoaiPhong.Content.ToString();

			// Kết hợp tất cả các điều kiện
			return matchTinhTrang && matchDonDep && matchLoaiPhong;
		}

		private void SaveFilterState()
		{
			// Lưu trạng thái của các bộ lọc
			lastTinhTrangSelection = spTrangThai.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true)
				?.Content.ToString();
			lastDonDepSelection = spDonDep.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true)
				?.Content.ToString();
			lastLoaiPhongSelection = spLoaiPhong.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true)
				?.Content.ToString();
		}

		private void RestoreFilterState()
		{
			// Khôi phục trạng thái của bộ lọc Tình Trạng
			if (lastTinhTrangSelection != null)
			{
				var radioTinhTrang = spTrangThai.Children.OfType<RadioButton>()
					.FirstOrDefault(r => r.Content.ToString() == lastTinhTrangSelection);
				if (radioTinhTrang != null)
				{
					radioTinhTrang.IsChecked = true;
					rb_Click(radioTinhTrang, null); // Gọi sự kiện Click để áp dụng bộ lọc
				}
			}

			// Khôi phục trạng thái của bộ lọc Dọn Dẹp
			if (lastDonDepSelection != null)
			{
				var radioDonDep = spDonDep.Children.OfType<RadioButton>()
					.FirstOrDefault(r => r.Content.ToString() == lastDonDepSelection);
				if (radioDonDep != null)
				{
					radioDonDep.IsChecked = true;
					rb_Click(radioDonDep, null); // Gọi sự kiện Click để áp dụng bộ lọc
				}
			}

			// Khôi phục trạng thái của bộ lọc Loại Phòng
			if (lastLoaiPhongSelection != null)
			{
				var radioLoaiPhong = spLoaiPhong.Children.OfType<RadioButton>()
					.FirstOrDefault(r => r.Content.ToString() == lastLoaiPhongSelection);
				if (radioLoaiPhong != null)
				{
					radioLoaiPhong.IsChecked = true;
					rb_Click(radioLoaiPhong, null); // Gọi sự kiện Click để áp dụng bộ lọc
				}
			}
		}

		private void timKiemTheomaPhong()
		{
			foreach (var listView in spDanhSachPhong.Children.OfType<ListView>())
			{
				if (listView.ItemsSource == null) continue;

				var view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
				view.Filter = filterTimKiem;
			}

			refreshListView();
		}

		private bool filterTimKiem(object obj)
		{
			if (string.IsNullOrEmpty(txbTimKiem.Text))
				return true;
			return (obj as Phong_Custom).MaPhong.Contains(txbTimKiem.Text);
		}

		private void refreshListView()
		{
			foreach (var listView in spDanhSachPhong.Children.OfType<ListView>())
				CollectionViewSource.GetDefaultView(listView.ItemsSource)?.Refresh();
		}

		private void capNhatLaiDuLieuListViewTheoNgayGio()
		{
			var dateTime = new DateTime();
			if (!DateTime.TryParse(dtpChonNgay.Text + " " + tpGio.Text, out dateTime))
			{
				new DialogCustoms("Nhập đúng định dạng ngày giờ !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			if (dateTime == null)
				new DialogCustoms("Chọn đúng định dạng tháng ngày năm!", "Ngày chọn", DialogCustoms.OK).ShowDialog();
			else
				lsPhong_Custom = PhongBUS.GetInstance().getDataPhongCustomTheoNgay(dateTime);

			refeshLoaiPhong();
			rdTatCaDonDep.IsChecked = true;
			rdTatCaTrangThai.IsChecked = true;
		}

		#endregion

		#region Event

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// Lưu trạng thái bộ lọc trước khi làm mới
			SaveFilterState();

			// Làm mới danh sách phòng và loại phòng
			spDanhSachPhong.UpdateLayout();
			refeshLoaiPhong();

			// Khôi phục lại trạng thái bộ lọc sau khi làm mới
			RestoreFilterState();
		}

		// Sự kiện loade UC
		private void ucPhong_Loaded(object sender, RoutedEventArgs e)
		{
			dtpChonNgay.Text = DateTime.Now.ToShortDateString();
			tpGio.Text = DateTime.Now.ToShortTimeString();
			if (lsPhong_Custom == null || !lsPhong_Custom.Any())
			{
				new DialogCustoms("Không có dữ liệu phòng để hiển thị!", "Thông báo", DialogCustoms.OK).Show();
				return;
			}

			refeshLoaiPhong();
		}

		//Khi click vào radioButton
		private void rb_Click(object sender, RoutedEventArgs e)
		{
			// Lọc danh sách phòng dựa trên các điều kiện
			foreach (var listView in spDanhSachPhong.Children.OfType<ListView>())
			{
				var view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
				if (view != null)
				{
					view.Filter = PhongFilter; // Áp dụng bộ lọc
					view.Refresh();
				}
			}
		}

		//Khi click vào 1 item trong LV
		private void ListViewPhong_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var lv = sender as ListView;
			var phong = lv.SelectedItem as Phong_Custom;
			if (phong != null)
			{
				var ct = new ChiTietPhong(MaNV);
				ct.truyenData(phong);
				if (ct.ShowDialog() == true) capNhatLaiDuLieuListViewTheoNgayGio();
				lv.UnselectAll();
			}
		}

		//Tìm kiếm theo mã phòng
		private void click_EnterSearch(object sender, RoutedEventArgs e)
		{
			timKiemTheomaPhong();
		}

		private void txbTimKiem_TextChanged(object sender, TextChangedEventArgs e)
		{
			timKiemTheomaPhong();
		}

		// Filter theo ngày tháng năm
		private void tpGio_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
		{
			capNhatLaiDuLieuListViewTheoNgayGio();
		}

		private void selectedDateChang_DatePicker(object sender, SelectionChangedEventArgs e)
		{
			capNhatLaiDuLieuListViewTheoNgayGio();
		}

		#endregion
	}
}