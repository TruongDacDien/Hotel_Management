using BUS;
using DAL.Data;
using DAL.DTO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for Them_SuaTaiKhoanNV.xaml
	/// </summary>
	public partial class Them_SuaTaiKhoanNV : Window
	{
		public delegate void suaData(TaiKhoanNV taiKhoan);

		public delegate bool truyenData(TaiKhoanNV taiKhoan);

		private readonly bool isEditing;

		private readonly ObservableCollection<NhanVien> list;

		public suaData suaTaiKhoan;

		public truyenData truyenTaiKhoan;

		public TaiKhoanNV taiKhoanNV;
		public string ImageId { get; set; }
		public string ImageURL { get; set; }
		public string SelectedImagePath { get; set; } //ảnh vừa chọn nhưng chưa upload
		public bool isDisabled { get; set; }

		public Them_SuaTaiKhoanNV()
		{
			InitializeComponent();
			list = new ObservableCollection<NhanVien>(NhanVienBUS.GetInstance().getDataNhanVien());
			cmbMaNV.ItemsSource = list;
			cmbMaNV.DisplayMemberPath = "DisplayInfo";
			cmbMaNV.SelectedValuePath = "MaNV";
			ckb_VoHieuHoaTaiKhoan.IsEnabled = false;
		}

		public Them_SuaTaiKhoanNV(bool isEditing = false, TaiKhoanNV taiKhoan = null) : this()
		{
			this.isEditing = isEditing;

			if (isEditing && taiKhoan != null)
			{
				ckb_VoHieuHoaTaiKhoan.IsEnabled = true;
				this.taiKhoanNV = taiKhoan;
				txtUsername.IsReadOnly = true;
				txtUsername.Text = taiKhoan.Username;
				txtEmail.IsReadOnly = true;
				txtEmail.Text = taiKhoan.Email;
				cmbMaNV.Text = taiKhoan.NhanVien.DisplayInfo;
				txbTitle.Text = "Sửa thông tin tài khoản " + taiKhoan.Username;
				ckb_VoHieuHoaTaiKhoan.IsChecked = taiKhoan.Disabled;
				this.ImageId = taiKhoan.AvatarId;
				this.ImageURL = taiKhoan.AvatarURL;
				LoadImage();
			}
			else
			{
				txbTitle.Text = "Nhập thông tin tài khoản";
			}
		}

		#region Load Image
		private void LoadImage()
		{
			try
			{
				var bitmap = new BitmapImage();

				//Bust cache bằng cách thêm chuỗi ngẫu nhiên vào URL
				var uri = new Uri($"{taiKhoanNV.AvatarURL}?v={Guid.NewGuid()}", UriKind.Absolute);

				bitmap.BeginInit();
				bitmap.UriSource = uri;
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache; // Cũng giúp không dùng cache
				bitmap.EndInit();

				imgAvatar.Fill = new ImageBrush(bitmap);
			}
			catch (Exception ex)
			{
				new DialogCustoms($"Không thể tải ảnh dịch vụ: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
			}
		}
		#endregion

		#region Update Image
		private string GenerateImageId()
		{
			if (taiKhoanNV != null && taiKhoanNV.MaTKNV > 0)
				return $"staff_{taiKhoanNV.MaTKNV}";
			return $"staff_temp_{Guid.NewGuid():N}";
		}

		private void click_ThayDoiAnh(object sender, RoutedEventArgs e)
		{
			var openFile = new OpenFileDialog
			{
				Filter = "Hình ảnh (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
				Title = "Chọn ảnh dịch vụ"
			};

			if (openFile.ShowDialog() == true)
			{
				try
				{
					this.SelectedImagePath = openFile.FileName; //lưu đường dẫn file để upload
					this.ImageId = GenerateImageId();
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
					string oldImageId = this.taiKhoanNV != null ? this.taiKhoanNV.AvatarId : null;
					// Upload lên Cloudinary → lấy URL
					var url = await TaiKhoanNVBUS.GetInstance().UploadAvatarAsync(this.SelectedImagePath, this.ImageId, oldImageId);
					this.ImageURL = url.ToString();
					if (this.taiKhoanNV != null)
					{
						this.ImageId = $"hotel_management/staff_{this.taiKhoanNV.MaTKNV}";
					}
				}
				catch (Exception ex)
				{
					new DialogCustoms($"Lỗi upload ảnh: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
				}
			}
		}
		#endregion

		#region Method
		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtUsername.Text) || txtUsername.Text.Any(ch => !char.IsLetterOrDigit(ch)))
			{
				new DialogCustoms("Vui lòng nhập tên tài khoản và không chứa ký tự đặc biệt", "Thông báo",
					DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtEmail.Text))
			{
				new DialogCustoms("Vui lòng nhập email", "Thông báo",
					DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(cmbMaNV.Text))
			{
				new DialogCustoms("Vui lòng chọn mã nhân viên", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			return true;
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

			if (this.isEditing)
			{
				var pass = string.Empty;
				if (string.IsNullOrWhiteSpace(txtPassword.Text))
					pass = TaiKhoanNVDAL.GetInstance().layTaiKhoanTheoUsername(txtUsername.Text).Password;
				else
					pass = Bcrypt_HashBUS.GetInstance().HashMatKhau(txtPassword.Text);
				var taiKhoan = new TaiKhoanNV
				{
					MaTKNV = this.taiKhoanNV.MaTKNV,
					Username = txtUsername.Text,
					Password = pass,
					AvatarId = this.ImageId,
					AvatarURL = this.ImageURL,
					Email = txtEmail.Text,
					Disabled = this.isDisabled,
					MaNV = int.Parse(cmbMaNV.SelectedValue.ToString())
				};

				if (suaTaiKhoan != null) suaTaiKhoan(taiKhoan);
			}
			else
			{
				btnThem_Click(sender, e);
			}

			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void VoHieuHoa_Checked(object sender, RoutedEventArgs e)
		{
			this.isDisabled = ckb_VoHieuHoaTaiKhoan.IsChecked == true;
		}

		private async void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			if (string.IsNullOrWhiteSpace(txtPassword.Text))
			{
				new DialogCustoms("Vui lòng nhập mật khẩu", "Thông báo", DialogCustoms.OK).Show();
				return;
			}

			if (TaiKhoanNVBUS.GetInstance().kiemTraTrungEmail(txtEmail.Text))
			{
				new DialogCustoms("Email đã được đăng ký!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			var pass = Bcrypt_HashBUS.GetInstance().HashMatKhau(txtPassword.Text);
			var taiKhoan = new TaiKhoanNV
			{
				Username = txtUsername.Text,
				Password = pass,
				AvatarId = this.ImageId,
				AvatarURL = this.ImageURL,
				Email = txtEmail.Text,
				MaNV = int.Parse(cmbMaNV.SelectedValue.ToString())
			};

			if (truyenTaiKhoan != null)
			{
				if (truyenTaiKhoan(taiKhoan))
				{
					// Nếu thêm thành công, cập nhật ImageId và ImageURL nếu có
					if (!string.IsNullOrEmpty(SelectedImagePath) && taiKhoan.MaTKNV > 0)
					{
						this.ImageId = $"staff_{taiKhoan.MaTKNV}";
						await UploadImageToCloudinary();
						this.ImageId = $"hotel_management/staff_{taiKhoan.MaTKNV}";
						TaiKhoanNVBUS.GetInstance().capNhatAvatar(taiKhoan.MaTKNV, this.ImageId, this.ImageURL);
					}
					var phanQuyen = new PhanQuyen
					{
						MaTKNV = taiKhoan.MaTKNV,
						TrangChu = true,
						Phong = true,
						DatPhong = true,
						QLDatDV = true,
						HoaDon = true,
						QLKhachHang = false,
						QLTaiKhoanKH = true,
						QLPhong = true,
						QLLoaiPhong = true,
						QLDichVu = true,
						QLLoaiDichVu = false,
						QLTienNghi = false,
						QLNhanVien = false,
						QLTaiKhoanNV = false,
						ThongKe = false,
						QLDDXQ = false,
					};
					TaiKhoanNVBUS.GetInstance().themPhanQuyenTaiKhoan(phanQuyen);
				}
				else
				{
					new DialogCustoms("Lỗi: Không thể tạo tài khoản nhân viên mới!", "Lỗi", DialogCustoms.OK).ShowDialog();
					return;
				}
			}
			var wd = GetWindow(sender as Button);
			wd.Close();
		}
		#endregion
	}
}