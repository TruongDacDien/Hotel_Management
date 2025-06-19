using BUS;
using DAL.DTO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for Them_SuaLoaiPhong.xaml
	/// </summary>
	public partial class Them_SuaLoaiPhong : Window
	{
		public delegate void suaData(LoaiPhong loaiPhong);

		public delegate void truyenData(LoaiPhong loaiPhong);

		private readonly string maLoaiPhong;
		public suaData suaLoaiPhong;
		public truyenData truyenLoaiPhong;
		private ObservableCollection<TienNghi> lsTienNghi_Customs;
		private ObservableCollection<TienNghi_DaChon> lsTienNghi_DaChon;
		private List<TienNghi> lsCache;

		public LoaiPhong loaiPhong { get; set; }
		public string ImageId { get; set; }
		public string ImageURL { get; set; }
		public string SelectedImagePath { get; set; } //ảnh vừa chọn nhưng chưa upload

		public Them_SuaLoaiPhong()
		{
			InitializeComponent();
		}

		#region Load Image
		private void LoadImage()
		{
			try
			{
				var bitmap = new BitmapImage();

				//Bust cache bằng cách thêm chuỗi ngẫu nhiên vào URL
				var uri = new Uri($"{loaiPhong.ImageURL}?v={Guid.NewGuid()}", UriKind.Absolute);

				bitmap.BeginInit();
				bitmap.UriSource = uri;
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache; // Cũng giúp không dùng cache
				bitmap.EndInit();

				imgAvatar.Fill = new ImageBrush(bitmap);
			}
			catch (Exception ex)
			{
				new DialogCustoms($"Không thể tải ảnh loại phòng: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
			}
		}
		#endregion

		#region Update Image
		private void click_ThayDoiAnh(object sender, RoutedEventArgs e)
		{
			var openFile = new OpenFileDialog
			{
				Filter = "Hình ảnh (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
				Title = "Chọn ảnh loại phòng"
			};

			if (openFile.ShowDialog() == true)
			{
				try
				{
					this.SelectedImagePath = openFile.FileName; //lưu đường dẫn file để upload
					this.ImageId = $"hotel_management/roomtype_{loaiPhong.MaLoaiPhong}";
					// Hiển thị lên giao diện
					var bitmap = new BitmapImage(new Uri(SelectedImagePath));
					imgAvatar.Fill = new ImageBrush(bitmap);
				}
				catch (Exception ex)
				{
					new DialogCustoms($"Không thể tải ảnh: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
				}
			}
		}
		#endregion

		#region Upload Image to Cloudinary
		private async void UploadImageToCloudinary()
		{
			// Upload avatar mới nếu người dùng có chọn
			if (!string.IsNullOrEmpty(SelectedImagePath))
			{
				try
				{
					// Upload lên Cloudinary → lấy URL
					var url = await LoaiPhongBUS.GetInstance().UploadImageAsync(this.SelectedImagePath, this.ImageId, this.loaiPhong.ImageId);
					this.ImageURL = url.ToString();
					this.ImageId = $"hotel_management/roomtype_{this.loaiPhong.MaLoaiPhong}"; // dùng đúng tên publicId
				}
				catch (Exception ex)
				{
					new DialogCustoms($"Lỗi upload ảnh: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
				}
			}
		}
		#endregion

		public Them_SuaLoaiPhong(LoaiPhong loaiPhong) : this()
		{
			lsTienNghi_Customs = new ObservableCollection<TienNghi>(TienNghiBUS.GetInstance().getDataTienNghi());
			lsTienNghi_DaChon = new ObservableCollection<TienNghi_DaChon>();
			lsCache = new List<TienNghi>();
			lvDanhSachTN.ItemsSource = lsTienNghi_Customs;
			lvTienNghiDaChon.ItemsSource = lsTienNghi_DaChon;
			this.loaiPhong = loaiPhong;
			txtTenLoaiPhong.IsReadOnly = true;
			txtTenLoaiPhong.Text = loaiPhong.TenLoaiPhong;
			txtMoTa.Text = loaiPhong.MoTa;
			txtChinhSach.Text = loaiPhong.ChinhSach;
			txtChinhSachHuy.Text = loaiPhong.ChinhSachHuy;
			txtSoNguoiToiDa.Text = loaiPhong.SoNguoiToiDa.ToString();
			txtGiaNgay.Text = loaiPhong.GiaNgay % 1 == 0
				? ((int)loaiPhong.GiaNgay).ToString()
				: loaiPhong.GiaNgay.ToString();
			txtGiaGio.Text = loaiPhong.GiaGio % 1 == 0
				? ((int)loaiPhong.GiaGio).ToString()
				: loaiPhong.GiaGio.ToString();
			txbTitle.Text = "Sửa thông tin " + loaiPhong.MaLoaiPhong;
			maLoaiPhong = loaiPhong.MaLoaiPhong.ToString();
			this.ImageId = loaiPhong.ImageId;
			this.ImageURL = loaiPhong.ImageURL;
			txtMoTa.Text = loaiPhong.MoTa;
			txtChinhSach.Text = loaiPhong.ChinhSach;
			txtChinhSachHuy.Text = loaiPhong.ChinhSachHuy;
			LoadImage();
		}

		#region Method

		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtTenLoaiPhong.Text))
			{
				new DialogCustoms("Vui lòng nhập tên loại phòng", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtMoTa.Text))
			{
				new DialogCustoms("Vui lòng nhập mô tả loại phòng", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtChinhSach.Text))
			{
				new DialogCustoms("Vui lòng nhập chính sách", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtChinhSachHuy.Text))
			{
				new DialogCustoms("Vui lòng nhập chính sách hủy", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtSoNguoiToiDa.Text))
			{
				new DialogCustoms("Vui lòng nhập số người tối đa", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtGiaNgay.Text))
			{
				new DialogCustoms("Vui lòng nhập giá ngày", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtGiaGio.Text))
			{
				new DialogCustoms("Vui lòng nhập giá giờ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			int so;
			if (int.TryParse(txtTenLoaiPhong.Text, out so))
			{
				new DialogCustoms("Vui lòng nhập đúng định dạng tên loại phòng", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (int.TryParse(txtSoNguoiToiDa.Text, out so) == false)
			{
				new DialogCustoms("Vui lòng nhập đúng định dạng số người tối đa", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (int.TryParse(txtGiaNgay.Text, out so) == false)
			{
				new DialogCustoms("Vui lòng nhập đúng định dạng giá ngày", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (int.TryParse(txtGiaGio.Text, out so) == false)
			{
				new DialogCustoms("Vui lòng nhập đúng định dạng giá giờ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			return true;
		}

		private void txbTimKiem_TextChanged(object sender, TextChangedEventArgs e)
		{
			var viewDV = (CollectionView)CollectionViewSource.GetDefaultView(lvDanhSachTN.ItemsSource);
			viewDV.Filter = filterTimKiem;
		}

		private bool filterTimKiem(object obj)
		{
			if (string.IsNullOrEmpty(txbTimKiem.Text))
				return true;
			var objTenTienNghi = RemoveVietnameseTone((obj as TienNghi).TenTN);
			var timkiem = RemoveVietnameseTone(txbTimKiem.Text);
			return objTenTienNghi.Contains(timkiem);
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

		private void txbSoLuong_LostFocus(object sender, RoutedEventArgs e)
		{
			var txb = sender as TextBox;
			var tndc = (sender as TextBox).DataContext as TienNghi_DaChon;
			var soLuong = 1;
			if (!int.TryParse(txb.Text, out soLuong))
			{
				new DialogCustoms("Lỗi: Nhập số lượng kiểu số nguyên!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			tndc.SoLuong = soLuong;
		}

		private void txbSoLuong_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var txb = sender as TextBox;
				var tndc = (sender as TextBox).DataContext as TienNghi_DaChon;
				var soLuong = 1;
				if (!int.TryParse(txb.Text, out soLuong))
				{
					new DialogCustoms("Lỗi: Nhập số lượng kiểu số nguyên!", "Thông báo", DialogCustoms.OK).ShowDialog();
					return;
				}

				tndc.SoLuong = soLuong;
			}
		}
		#endregion

		#region Event

		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			UploadImageToCloudinary();

			var loaiPhong = new LoaiPhong
			{
				MaLoaiPhong = int.Parse(maLoaiPhong),
				TenLoaiPhong = txtTenLoaiPhong.Text,
				MoTa = txtMoTa.Text,
				ChinhSach= txtChinhSach.Text,
				ChinhSachHuy= txtChinhSachHuy.Text,
				ImageId= this.ImageId,
				ImageURL= this.ImageURL,
				SoNguoiToiDa = int.Parse(txtSoNguoiToiDa.Text),
				GiaGio = decimal.Parse(txtGiaGio.Text),
				GiaNgay = decimal.Parse(txtGiaNgay.Text)
			};
			if (suaLoaiPhong != null) suaLoaiPhong(loaiPhong);

			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			UploadImageToCloudinary();

			var loaiPhong = new LoaiPhong
			{
				TenLoaiPhong = txtTenLoaiPhong.Text,
				MoTa = txtMoTa.Text,
				ChinhSach = txtChinhSach.Text,
				ChinhSachHuy = txtChinhSachHuy.Text,
				ImageId = this.ImageId,
				ImageURL = this.ImageURL,
				SoNguoiToiDa = int.Parse(txtSoNguoiToiDa.Text),
				GiaGio = decimal.Parse(txtGiaGio.Text),
				GiaNgay = decimal.Parse(txtGiaNgay.Text)
			};
			if (truyenLoaiPhong != null) truyenLoaiPhong(loaiPhong);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void click_Them(object sender, RoutedEventArgs e)
		{
			var tnct = (sender as Button).DataContext as TienNghi;
			lsTienNghi_DaChon.Add(new TienNghi_DaChon{ MaTN=tnct.MaTN, TenTN = tnct.TenTN, SoLuong = 1 });
			lsCache.Add(tnct);
			lsTienNghi_Customs.Remove(tnct);
		}

		private void click_Xoa(object sender, RoutedEventArgs e)
		{
			var tndachon = (sender as Button).DataContext as TienNghi_DaChon;
			var tienNghi_Custom = lsCache.Where(p => p.MaTN.Equals(tndachon.MaTN)).FirstOrDefault();
			lsTienNghi_Customs.Add(tienNghi_Custom);
			lsTienNghi_DaChon.Remove(tndachon);
		}
		#endregion
	}
}