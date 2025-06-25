using BUS;
using DAL.DTO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

		public delegate bool truyenData(LoaiPhong loaiPhong);

		private readonly string maLoaiPhong;
		public suaData suaLoaiPhong;
		public truyenData truyenLoaiPhong;
		private ObservableCollection<TienNghi> lsTienNghi_Customs;
		private ObservableCollection<TienNghi_DaChon> lsTienNghi_DaChon;
		private List<TienNghi> lsCache;
		private List<CT_TienNghi> lsTienNghiPhong;

		public LoaiPhong loaiPhong { get; set; }
		public string ImageId { get; set; }
		public string ImageURL { get; set; }
		public string SelectedImagePath { get; set; } //ảnh vừa chọn nhưng chưa upload

		public Them_SuaLoaiPhong()
		{
			InitializeComponent();
			lsTienNghi_Customs = new ObservableCollection<TienNghi>(TienNghiBUS.GetInstance().getDataTienNghi());
			lsTienNghi_DaChon = new ObservableCollection<TienNghi_DaChon>();
			lsCache = new List<TienNghi>();
			foreach (var tn in lsTienNghi_Customs)
			{
				tn.SoLuongBanDau = tn.SoLuong;
			}
			lvDanhSachTN.ItemsSource = lsTienNghi_Customs;
			lvTienNghiDaChon.ItemsSource = lsTienNghi_DaChon;
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
					// Tạo ImageId tạm thời nếu MaLoaiPhong chưa có
					this.ImageId = (string.IsNullOrEmpty(maLoaiPhong) || maLoaiPhong == "0")
									? $"roomtype_temp_{Guid.NewGuid():N}"
									: $"roomtype_{maLoaiPhong}";
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
		private async Task UploadImageToCloudinary()
		{
			// Upload avatar mới nếu người dùng có chọn
			if (!string.IsNullOrEmpty(SelectedImagePath))
			{
				try
				{
					string oldImageId = this.loaiPhong != null ? this.loaiPhong.ImageId : null;
					// Upload lên Cloudinary → lấy URL
					var url = await LoaiPhongBUS.GetInstance().UploadImageAsync(this.SelectedImagePath, this.ImageId, oldImageId);
					this.ImageURL = url.ToString();
					// Cập nhật ImageId theo chuẩn nếu đã có MaLoaiPhong (chỉ trong sửa)
					if (this.loaiPhong != null)
					{
						this.ImageId = $"hotel_management/roomtype_{this.loaiPhong.MaLoaiPhong}";
					}
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
			lsTienNghiPhong = new List<CT_TienNghi>(CT_TienNghiBUS.GetInstance().getCTTienNghiByLoaiPhong(loaiPhong.MaLoaiPhong));
			lsTienNghi_Customs = new ObservableCollection<TienNghi>(TienNghiBUS.GetInstance().getDataTienNghi());
			lsTienNghi_DaChon = new ObservableCollection<TienNghi_DaChon>();
			lsCache = new List<TienNghi>();
			foreach (var tn in lsTienNghi_Customs)
			{
				tn.SoLuongBanDau = tn.SoLuong;
			}
			lvDanhSachTN.ItemsSource = lsTienNghi_Customs;
			foreach (var ct in lsTienNghiPhong)
			{
				var tnDaChon = new TienNghi_DaChon
				{
					MaTN = ct.MaTN,
					TenTN = ct.TenTN,
					SoLuong = ct.SL,
					SoLuongTruKhoLucThem = ct.SL // Track initial quantity used
				};
				lsTienNghi_DaChon.Add(tnDaChon);
			}
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
			txbSoLuong_KeyUp(sender, new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter));
		}

		private void txbSoLuong_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var txb = sender as TextBox;
				var tndc = txb.DataContext as TienNghi_DaChon;

				if (!int.TryParse(txb.Text, out int newSoLuong) || newSoLuong < 0)
				{
					new DialogCustoms("Nhập số nguyên không âm!", "Thông báo", DialogCustoms.OK).ShowDialog();
					txb.Text = tndc.SoLuong.ToString();
					return;
				}

				var tnCustom = lsTienNghi_Customs.FirstOrDefault(t => t.MaTN == tndc.MaTN);
				if (tnCustom == null) return;

				int oldSoLuong = (int)tndc.SoLuong;
				int delta = newSoLuong - oldSoLuong;

				// Check if enough stock is available
				if (tnCustom.SoLuong - delta < 0)
				{
					new DialogCustoms("Không đủ tiện nghi trong kho!", "Thông báo", DialogCustoms.OK).ShowDialog();
					txb.Text = tndc.SoLuong.ToString();
					return;
				}

				// Update quantities
				tndc.SoLuong = newSoLuong;
				tndc.SoLuongTruKhoLucThem += delta; // Update tracked quantity
				tnCustom.SoLuong -= delta;
			}
		}

		private void LuuDanhSachTienNghi(int maLoaiPhong)
		{
			try
			{
				// Lấy danh sách tiện nghi hiện tại trong CT_TienNghi từ database
				var existingCTs = CT_TienNghiBUS.GetInstance().getCTTienNghiByLoaiPhong(maLoaiPhong);

				// Xóa các tiện nghi không còn trong lsTienNghi_DaChon
				foreach (var ct in existingCTs)
				{
					if (!lsTienNghi_DaChon.Any(tn => tn.MaTN == ct.MaTN))
					{
						// Xóa bản ghi trong CT_TienNghi
						CT_TienNghiBUS.GetInstance().xoaCTTienNghi(ct);

						// Hoàn lại số lượng vào bảng TienNghi
						TienNghiBUS.GetInstance().capNhatSoLuongTienNghi(ct.MaTN, ct.SL);
					}
				}

				// Thêm hoặc cập nhật tiện nghi trong lsTienNghi_DaChon
				foreach (var tn in lsTienNghi_DaChon)
				{
					var ct = new CT_TienNghi
					{
						MaLoaiPhong = maLoaiPhong,
						MaTN = tn.MaTN,
						TenTN = tn.TenTN,
						SL = (int)tn.SoLuong
					};

					// Tìm bản ghi hiện tại trong database để tính sự thay đổi số lượng
					var existingCT = existingCTs.FirstOrDefault(c => c.MaTN == tn.MaTN);
					int soLuongThayDoi = existingCT != null ? ct.SL - existingCT.SL : ct.SL;

					if (CT_TienNghiBUS.GetInstance().KiemTraTonTai(ct))
					{
						// Cập nhật bản ghi trong CT_TienNghi
						CT_TienNghiBUS.GetInstance().capNhatCTTienNghi(ct);
					}
					else
					{
						// Thêm mới bản ghi vào CT_TienNghi
						CT_TienNghiBUS.GetInstance().addCTTienNghi(ct);
					}

					// Cập nhật số lượng trong bảng TienNghi (giảm số lượng)
					if (soLuongThayDoi != 0)
					{
						TienNghiBUS.GetInstance().capNhatSoLuongTienNghi(tn.MaTN, -soLuongThayDoi);
					}
				}
			}
			catch (Exception ex)
			{
				new DialogCustoms($"Lỗi khi lưu danh sách tiện nghi: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
			}
		}
		#endregion

		#region Event
		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private async void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			await UploadImageToCloudinary();

			var loaiPhong = new LoaiPhong
			{
				MaLoaiPhong = int.Parse(maLoaiPhong),
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
			if (suaLoaiPhong != null)
			{
				suaLoaiPhong(loaiPhong);
				LuuDanhSachTienNghi(loaiPhong.MaLoaiPhong);
			}

			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private async void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

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

			if (truyenLoaiPhong != null)
			{
				if (truyenLoaiPhong(loaiPhong))
				{
					// Nếu thêm thành công, cập nhật ImageId và ImageURL nếu có
					if (!string.IsNullOrEmpty(SelectedImagePath) && loaiPhong.MaLoaiPhong > 0)
					{
						this.ImageId = $"roomtype_{loaiPhong.MaLoaiPhong}";
						await UploadImageToCloudinary();
						this.ImageId = $"hotel_management/roomtype_{loaiPhong.MaLoaiPhong}";
						LoaiPhongBUS.GetInstance().capNhatHinhAnhLoaiPhong(loaiPhong.MaLoaiPhong, this.ImageId, this.ImageURL);
					}
					LuuDanhSachTienNghi(loaiPhong.MaLoaiPhong);
				}
				else
				{
					new DialogCustoms("Lỗi: Không thể tạo loại phòng mới!", "Lỗi", DialogCustoms.OK).ShowDialog();
					return;
				}
			}

			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void click_Them(object sender, RoutedEventArgs e)
		{
			var tnct = (sender as Button).DataContext as TienNghi;
			if (tnct != null && tnct.SoLuong <= 0)
			{
				new DialogCustoms("Đã hết tiện nghi trong kho", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			// Check if enough stock is available
			if (tnct.SoLuong < 1)
			{
				new DialogCustoms("Không đủ tiện nghi trong kho!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			tnct.SoLuong--;

			var daChon = lsTienNghi_DaChon.FirstOrDefault(t => t.MaTN == tnct.MaTN);
			if (daChon != null)
			{
				daChon.SoLuong++;
				daChon.SoLuongTruKhoLucThem++; // Increment tracked quantity
			}
			else
			{
				lsTienNghi_DaChon.Add(new TienNghi_DaChon
				{
					MaTN = tnct.MaTN,
					TenTN = tnct.TenTN,
					SoLuong = 1,
					SoLuongTruKhoLucThem = 1
				});
			}
		}

		private void click_Xoa(object sender, RoutedEventArgs e)
		{
			var tndachon = (sender as Button).DataContext as TienNghi_DaChon;
			if (tndachon == null) return;

			var tnCustom = lsTienNghi_Customs.FirstOrDefault(t => t.MaTN == tndachon.MaTN);
			if (tnCustom != null)
			{
				// Restore the quantity used by this amenity
				tnCustom.SoLuong += tndachon.SoLuongTruKhoLucThem;
			}
			else
			{
				// If not found, create a new entry
				lsTienNghi_Customs.Add(new TienNghi
				{
					MaTN = tndachon.MaTN,
					TenTN = tndachon.TenTN,
					SoLuong = tndachon.SoLuongTruKhoLucThem,
					SoLuongBanDau = tndachon.SoLuongTruKhoLucThem
				});
			}

			lsTienNghi_DaChon.Remove(tndachon);
		}
		#endregion
	}
}