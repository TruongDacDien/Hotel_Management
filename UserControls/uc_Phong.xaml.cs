using System;
using System.Collections.Generic;
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
using GUI.View;
using BUS;
using DAL.Data;
using DAL.DTO;
using System.Collections.ObjectModel;
using System.Windows.Forms.VisualStyles;

namespace GUI.UserControls
{
    /// <summary>
    /// Interaction logic for uc_Phong.xaml
    /// </summary>
    public partial class uc_Phong : UserControl
    {
        ObservableCollection<Phong_Custom> lsPhong_Custom;
        ObservableCollection<Phong_Custom> lsTrong;
        private int maNV;
        public int MaNV { get => maNV; set => maNV = value; }

        private uc_Phong()
        {
            InitializeComponent();
            lsTrong = new ObservableCollection<Phong_Custom>();
			lsPhong_Custom = PhongBUS.GetInstance().getDataPhong_Custom();
			initEvent();
        }

        public uc_Phong(int maNV):this()
        {
            this.MaNV = maNV;
        }


		#region Method
		private void ThemRadioButtonLoaiPhong(string loaiPhong)
		{
			// Kiểm tra xem radio button đã tồn tại chưa
			if (spLoaiPhong.Children.OfType<RadioButton>().Any(rb => rb.Content.ToString() == loaiPhong)) return;
			// Tạo mới radio button
			RadioButton newRadioButton = new RadioButton
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
			TextBlock title = new TextBlock
			{
				Text = loaiPhong,
				FontSize = 20,
				Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00A3FF")),
				Margin = new Thickness(40, 20, 0, 0)
			};

			// Lọc danh sách phòng theo loại phòng
			var filteredPhong = lsPhong_Custom.Where(p => p.LoaiPhong == loaiPhong).ToList();

			// Tạo ListView cho phòng
			ListView listView = new ListView
			{
				Margin = new Thickness(20, 10, 20, 20),
				ItemsSource = new ObservableCollection<Phong_Custom>(filteredPhong), // Gán danh sách phòng đã lọc
				ItemTemplate = (DataTemplate)this.Resources["PhongItemTemplate"], // Sử dụng DataTemplate đã khai báo
				HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
				VerticalAlignment = System.Windows.VerticalAlignment.Top,
				Name = $"lv{loaiPhong.Replace(" ", "")}" // Đặt tên cho ListView để dễ dàng quản lý
			};

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
			RadioButton rbTatCaLoaiPhong = new RadioButton
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
				ThemDanhSachPhong(loaiPhong);       // Tạo danh sách phòng tương ứng
			}
		}

		private void initEvent()
        {
			// Gắn sự kiện Click cho RadioButton trong spTrangThai
			foreach (var radioButton in spTrangThai.Children.OfType<RadioButton>())
			{
				radioButton.Click += rb_Click;
			}

			// Gắn sự kiện Click cho RadioButton trong spDonDep
			foreach (var radioButton in spDonDep.Children.OfType<RadioButton>())
			{
				radioButton.Click += rb_Click;
			}
		}

		private bool PhongFilter(object obj)
		{
			if (!(obj is Phong_Custom ph)) return false;

			// Lấy các RadioButton được chọn
			var radioTinhTrang = spTrangThai.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
			var radioDonDep = spDonDep.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
			var radioLoaiPhong = spLoaiPhong.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);

			// Điều kiện lọc theo Tình Trạng
			bool matchTinhTrang = radioTinhTrang == null || radioTinhTrang.Content.ToString() == "Tất cả phòng" || ph.TinhTrang == radioTinhTrang.Content.ToString();

			// Điều kiện lọc theo Dọn Dẹp
			bool matchDonDep = radioDonDep == null || radioDonDep.Content.ToString() == "Tất cả" || ph.DonDep == radioDonDep.Content.ToString();

			// Điều kiện lọc theo Loại Phòng
			bool matchLoaiPhong = radioLoaiPhong == null || radioLoaiPhong.Content.ToString() == "Tất cả loại phòng" || ph.LoaiPhong == radioLoaiPhong.Content.ToString();

			// Kết hợp tất cả các điều kiện
			return matchTinhTrang && matchDonDep && matchLoaiPhong;
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
            if (String.IsNullOrEmpty(txbTimKiem.Text))
                return true;
            else
                return (obj as Phong_Custom).MaPhong.Contains(txbTimKiem.Text);
        }

		private void refreshListView()
		{
			foreach (var listView in spDanhSachPhong.Children.OfType<ListView>())
			{
				CollectionViewSource.GetDefaultView(listView.ItemsSource)?.Refresh();
			}
		}

		private void capNhatLaiDuLieuListViewTheoNgayGio()
        {
            DateTime dateTime = new DateTime();
            if (!DateTime.TryParse(dtpChonNgay.Text + " " + tpGio.Text, out dateTime))
            {
                new DialogCustoms("Nhập đúng định dạng ngày giờ !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return;
            }
            if (dateTime == null)
            {
                new DialogCustoms("Chọn đúng định dạng tháng ngày năm!", "Ngày chọn", DialogCustoms.OK).ShowDialog();
            }
            else
            {
                lsPhong_Custom = PhongBUS.GetInstance().getDataPhongCustomTheoNgay(dateTime);
            }

            refeshLoaiPhong();
            rdTatCaDonDep.IsChecked = true;
            rdTatCaTrangThai.IsChecked = true;
        }

		#endregion

		#region Event
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
			ListView lv = sender as ListView;
			Phong_Custom phong = lv.SelectedItem as Phong_Custom;
			if (phong != null)
			{
				ChiTietPhong ct = new ChiTietPhong(maNV);
				ct.truyenData(phong);
				if (ct.ShowDialog() == true)
				{
					capNhatLaiDuLieuListViewTheoNgayGio();
				}
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
