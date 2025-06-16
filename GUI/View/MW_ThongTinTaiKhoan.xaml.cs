using DAL.DTO;
using BUS;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace GUI.View
{
	public partial class MW_ThongTinTaiKhoan : Window
	{
		public TaiKhoanNV TaiKhoan { get; set; }
		public string AvatarId { get; set; }
		public string AvatarURL { get; set; }
		public string SelectedAvatarPath { get; set; } //ảnh vừa chọn nhưng chưa upload

		private bool _isPasswordVisible;
		public bool IsPasswordVisible
		{
			get => _isPasswordVisible;
			set
			{
				_isPasswordVisible = value;
				UpdatePasswordVisibility();
			}
		}

		public MW_ThongTinTaiKhoan()
		{
			InitializeComponent();
		}

		#region Constructor
		public MW_ThongTinTaiKhoan(TaiKhoanNV taiKhoan) : this()
		{
			TaiKhoan = taiKhoan;
			LoadAvatar();
			AvatarId = TaiKhoan.AvatarId;
			AvatarURL = TaiKhoan.AvatarURL;
			txtUsername.Text = TaiKhoan.Username;
			txtUsername.IsReadOnly = true;
			txtEmail.Text = TaiKhoan.Email;
			txtEmail.IsReadOnly = true;
			txtPasswordHidden.Password = string.Empty;
			txtPasswordVisible.Text = string.Empty;
		}
		#endregion

		#region Load Avatar
		private void LoadAvatar()
		{
			try
			{
				var bitmap = new BitmapImage();

				//Bust cache bằng cách thêm chuỗi ngẫu nhiên vào URL
				var uri = new Uri($"{TaiKhoan.AvatarURL}?v={Guid.NewGuid()}", UriKind.Absolute);

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
		#endregion

		#region Update Avatar
		private void click_ThayDoiAnh(object sender, RoutedEventArgs e)
		{
			var openFile = new OpenFileDialog
			{
				Filter = "Hình ảnh (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
				Title = "Chọn ảnh đại diện"
			};

			if (openFile.ShowDialog() == true)
			{
				try
				{
					this.SelectedAvatarPath = openFile.FileName; //lưu đường dẫn file để upload
					this.AvatarId = $"hotel_management/avatar_{TaiKhoan.Username}";
					// Hiển thị lên giao diện
					var bitmap = new BitmapImage(new Uri(SelectedAvatarPath));
					imgAvatar.Fill = new ImageBrush(bitmap);
				}
				catch (Exception ex)
				{
					new DialogCustoms($"Không thể tải ảnh: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
				}
			}
		}
		#endregion

		#region Update Account
		private async void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidateInput())
				return;

			string error = string.Empty;

			// Cập nhật mật khẩu
			if (!string.IsNullOrWhiteSpace(txtPasswordVisible.Text))
			{
				this.TaiKhoan.Password = Bcrypt_HashBUS.GetInstance().HashMatKhau(txtPasswordVisible.Text);
			}

			// Upload avatar mới nếu người dùng có chọn
			if (!string.IsNullOrEmpty(SelectedAvatarPath))
			{
				try
				{
					// Upload lên Cloudinary → lấy URL
					var url = await TaiKhoanBUS.GetInstance().UploadAvatarAsync(this.SelectedAvatarPath, this.AvatarId, TaiKhoan.AvatarId);
					this.AvatarURL = url.ToString();
					this.AvatarId = $"hotel_management/avatar_{TaiKhoan.Username}"; // dùng đúng tên publicId
				}
				catch (Exception ex)
				{
					new DialogCustoms($"Lỗi upload ảnh: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
				}
			}

			// Cập nhật thông tin tài khoản
			bool isAccountUpdated = TaiKhoanBUS.GetInstance().capNhatTaiKhoan(this.TaiKhoan);

			// Cập nhật ảnh đại diện
			bool isAvatarUpdated = TaiKhoanBUS.GetInstance().capNhatAvatar(this.TaiKhoan.Username, this.AvatarId, this.AvatarURL, out error);
			if (isAvatarUpdated)
			{
				TaiKhoan.AvatarURL = this.AvatarURL; // Gán lại ảnh đại diện mới vào đối tượng
				LoadAvatar(); // Gọi lại phương thức LoadAvatar để hiển thị ảnh mới
			}

			// Thông báo kết quả
			ShowUpdateResult(isAccountUpdated, isAvatarUpdated, error);
			Close();
		}

		private bool ValidateInput()
		{
			if (string.IsNullOrWhiteSpace(txtUsername.Text))
			{
				new DialogCustoms("Tên tài khoản không được để trống!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			if (txtUsername.Text.Any(ch => !char.IsLetterOrDigit(ch)))
			{
				new DialogCustoms("Tên tài khoản không được chứa ký tự đặc biệt!", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			return true;
		}

		private void ShowUpdateResult(bool isAccountUpdated, bool isAvatarUpdated, string error)
		{
			if (isAccountUpdated && isAvatarUpdated)
			{
				new DialogCustoms("Cập nhật tài khoản và ảnh đại diện thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
			}
			else if (!isAccountUpdated && isAvatarUpdated)
			{
				new DialogCustoms("Cập nhật ảnh đại diện thành công!\nCập nhật tài khoản thất bại!", "Thông báo", DialogCustoms.OK).ShowDialog();
			}
			else if (isAccountUpdated && !isAvatarUpdated)
			{
				new DialogCustoms($"Cập nhật tài khoản thành công!\nThay đổi ảnh đại diện thất bại: {error}", "Thông báo", DialogCustoms.OK).ShowDialog();
			}
			else
			{
				new DialogCustoms($"Cập nhật thất bại!\nLỗi: {error}", "Thông báo", DialogCustoms.OK).ShowDialog();
			}
		}
		#endregion

		#region Password Visibility
		private void Hide_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			IsPasswordVisible = !IsPasswordVisible;
			UpdatePasswordVisibility();
		}

		private void txtPasswordHidden_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (!IsPasswordVisible)
			{
				txtPasswordVisible.Text = txtPasswordHidden.Password;
			}
		}

		private void UpdatePasswordVisibility()
		{
			if (IsPasswordVisible)
			{
				txtPasswordVisible.Text = txtPasswordHidden.Password;
				txtPasswordHidden.Visibility = Visibility.Collapsed;
				txtPasswordVisible.Visibility = Visibility.Visible;
				iconEye.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;
			}
			else
			{
				txtPasswordHidden.Password = txtPasswordVisible.Text;
				txtPasswordVisible.Visibility = Visibility.Collapsed;
				txtPasswordHidden.Visibility = Visibility.Visible;
				iconEye.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;
			}
		}
		#endregion

		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
