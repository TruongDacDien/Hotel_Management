using BUS;
using DAL.Data;
using DAL.DTO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace GUI.View
{
	/// <summary>
	/// Interaction logic for Them_SuaTaiKhoanKH.xaml
	/// </summary>
	public partial class Them_SuaTaiKhoanKH : Window
	{
		public delegate void suaData(TaiKhoanKH taiKhoan);
		public delegate bool truyenData(TaiKhoanKH taiKhoan);

		private readonly bool isEditing;
		private readonly ObservableCollection<KhachHang> list;

		public suaData suaTaiKhoan;
		public truyenData truyenTaiKhoan;

		public TaiKhoanKH taiKhoanKH;
		public string ImageId { get; set; }
		public string ImageURL { get; set; }
		public string SelectedImagePath { get; set; }
		public bool isDisabled { get; set; }

		public Them_SuaTaiKhoanKH()
		{
			InitializeComponent();
			list = new ObservableCollection<KhachHang>(KhachHangBUS.GetInstance().GetKhachHangs());
			cmbMaKH.ItemsSource = list;
			cmbMaKH.DisplayMemberPath = "DisplayInfo";
			cmbMaKH.SelectedValuePath = "MaKH";
			ckb_VoHieuHoaTaiKhoan.IsEnabled = false;
		}

		public Them_SuaTaiKhoanKH(bool isEditing = false, TaiKhoanKH taiKhoan = null) : this()
		{
			this.isEditing = isEditing;

			if (isEditing && taiKhoan != null)
			{
				ckb_VoHieuHoaTaiKhoan.IsEnabled = true;
				this.taiKhoanKH = taiKhoan;
				txtUsername.IsReadOnly = true;
				txtUsername.Text = taiKhoan.Username;
				txtEmail.IsReadOnly = true;
				txtEmail.Text = taiKhoan.Email;
				cmbMaKH.Text = taiKhoan.KhachHang.DisplayInfo;
				txbTitle.Text = "Sửa thông tin tài khoản " + taiKhoan.Username;
				ckb_VoHieuHoaTaiKhoan.IsChecked = taiKhoan.Disabled;
				this.ImageId = taiKhoan.AvatarId;
				this.ImageURL = taiKhoan.AvatarURL;
				LoadImage();
			}
			else
			{
				txbTitle.Text = "Nhập thông tin tài khoản khách hàng";
			}
		}

		#region Load Image
		private void LoadImage()
		{
			try
			{
				var bitmap = new BitmapImage();

				//Bust cache bằng cách thêm chuỗi ngẫu nhiên vào URL
				var uri = new Uri($"{taiKhoanKH.AvatarURL}?v={Guid.NewGuid()}", UriKind.Absolute);

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
			if (taiKhoanKH != null && taiKhoanKH.MaTKKH > 0)
				return $"customer_{taiKhoanKH.MaTKKH}";
			return $"customer_temp_{Guid.NewGuid():N}";
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
					string oldImageId = this.taiKhoanKH != null ? this.taiKhoanKH.AvatarId : null;
					// Upload lên Cloudinary → lấy URL
					var url = await TaiKhoanKHBUS.GetInstance().UploadAvatarAsync(this.SelectedImagePath, this.ImageId, oldImageId);
					this.ImageURL = url.ToString();
					if (this.taiKhoanKH != null)
					{
						this.ImageId = $"hotel_management/customer_{this.taiKhoanKH.MaTKKH}";
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
				new DialogCustoms("Vui lòng nhập tên tài khoản và không chứa ký tự đặc biệt", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtEmail.Text))
			{
				new DialogCustoms("Vui lòng nhập email", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(cmbMaKH.Text))
			{
				new DialogCustoms("Vui lòng chọn mã khách hàng", "Thông báo", DialogCustoms.OK).Show();
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
					pass = TaiKhoanKHDAL.GetInstance().layTaiKhoanTheoUsername(txtUsername.Text).Password;
				else
					pass = Bcrypt_HashBUS.GetInstance().HashMatKhau(txtPassword.Text);
				var taiKhoan = new TaiKhoanKH
				{
					MaTKKH = this.taiKhoanKH.MaTKKH,
					Username = txtUsername.Text,
					Password = pass,
					AvatarId = this.ImageId,
					AvatarURL = this.ImageURL,
					Email = txtEmail.Text,
					Disabled = this.isDisabled,
					MaKH = int.Parse(cmbMaKH.SelectedValue.ToString())
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

			if (TaiKhoanKHBUS.GetInstance().kiemTraTrungEmail(txtEmail.Text))
			{
				new DialogCustoms("Email đã được đăng ký!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			var pass = Bcrypt_HashBUS.GetInstance().HashMatKhau(txtPassword.Text);
			var taiKhoan = new TaiKhoanKH
			{
				Username = txtUsername.Text,
				Password = pass,
				AvatarId = this.ImageId,
				AvatarURL = this.ImageURL,
				Email = txtEmail.Text,
				MaKH = int.Parse(cmbMaKH.SelectedValue.ToString())
			};

			if (truyenTaiKhoan != null)
			{
				if (truyenTaiKhoan(taiKhoan))
				{
					// Nếu thêm thành công, cập nhật ImageId và ImageURL nếu có
					if (!string.IsNullOrEmpty(SelectedImagePath) && taiKhoan.MaTKKH > 0)
					{
						this.ImageId = $"customer_{taiKhoan.MaTKKH}";
						await UploadImageToCloudinary();
						this.ImageId = $"hotel_management/customer_{taiKhoan.MaTKKH}";
						TaiKhoanKHBUS.GetInstance().capNhatAvatar(taiKhoan.MaTKKH, this.ImageId, this.ImageURL);
					}
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
