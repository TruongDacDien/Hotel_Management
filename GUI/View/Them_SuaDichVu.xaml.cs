using BUS;
using DAL.DTO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for Them_SuaDichVu.xaml
	/// </summary>
	public partial class Them_SuaDichVu : Window
	{
		public delegate void SuaDuLieu(DichVu dv);

		public delegate void TryenDuLieu(DichVu dv);

		private readonly bool isEditing;
		private readonly string maDV;
		private List<LoaiDV> loaiDV;
		public SuaDuLieu sua;
		public TryenDuLieu truyen;

		public DichVu dichVu { get; set; }
		public string ImageId { get; set; }
		public string ImageURL { get; set; }
		public string SelectedImagePath { get; set; } //ảnh vừa chọn nhưng chưa upload

		public Them_SuaDichVu()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		#region Load Image
		private void LoadImage()
		{
			try
			{
				var bitmap = new BitmapImage();

				//Bust cache bằng cách thêm chuỗi ngẫu nhiên vào URL
				var uri = new Uri($"{dichVu.ImageURL}?v={Guid.NewGuid()}", UriKind.Absolute);

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
					this.ImageId = $"hotel_management/service_{dichVu.MaDV}";
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
					var url = await DichVuBUS.GetInstance().UploadImageAsync(this.SelectedImagePath, this.ImageId, this.dichVu.ImageId);
					this.ImageURL = url.ToString();
					this.ImageId = $"hotel_management/service_{this.dichVu.MaDV}"; // dùng đúng tên publicId
				}
				catch (Exception ex)
				{
					new DialogCustoms($"Lỗi upload ảnh: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
				}
			}
		}
		#endregion

		public Them_SuaDichVu(bool isEditing = false, DichVu dv = null) : this()
		{
			this.isEditing = isEditing;
			txtTenDichVu.IsReadOnly = false;
			cmbMaLoai.DisplayMemberPath = "TenLoaiDV";
			cmbMaLoai.SelectedValuePath = "MaLoaiDV";

			if (isEditing && dv != null)
			{
				this.dichVu = dv;
				txtTenDichVu.Text = dv.TenDV;
				cmbMaLoai.Text = dv.TenLoaiDV;
				txtGia.Text = dv.Gia % 1 == 0 ? ((int)dv.Gia).ToString() : dv.Gia.ToString();
				//txtGia.Text = dv.Gia.ToString();
				txbTitle.Text = "Sửa thông tin dịch vụ " + dv.MaDV;
				maDV = dv.MaDV.ToString();
				txtMoTaDichVu.Text = dv.MoTa;
				txtSoLuong.Text = dv.SoLuong.ToString();
			}
			else
			{
				txbTitle.Text = "Nhập thông tin dịch vụ";
			}
		}

		private void TaiDanhSach()
		{
			loaiDV = new List<LoaiDV>(LoaiDichVuBUS.GetInstance().getDataLoaiDV());
			Console.WriteLine($"Số lượng loại DV: {loaiDV.Count}");

			cmbMaLoai.ItemsSource = loaiDV;
			cmbMaLoai.DisplayMemberPath = "TenLoaiDV";
			cmbMaLoai.SelectedValuePath = "MaLoaiDV";
		}

		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private async void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			UploadImageToCloudinary();

			if (isEditing)
			{
				var dichVu = new DichVu
				{
					MaDV = int.Parse(maDV),
					TenDV = txtTenDichVu.Text,
					MoTa = txtMoTaDichVu.Text,
					Gia = int.Parse(txtGia.Text),
					SoLuong = int.Parse(txtSoLuong.Text),
					MaLoaiDV = (int)cmbMaLoai.SelectedValue,
					ImageId = this.ImageId,
					ImageURL = this.ImageURL,
				};
				if (sua != null) sua(dichVu);
			}
			else
			{
				btnThem_Click(sender, e);
			}

			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var dichVu = new DichVu
			{
				TenDV = txtTenDichVu.Text,
				MoTa = txtMoTaDichVu.Text,
				Gia = int.Parse(txtGia.Text),
				SoLuong = int.Parse(txtSoLuong.Text),
				MaLoaiDV = (int)cmbMaLoai.SelectedValue,
				ImageId = this.ImageId,
				ImageURL = this.ImageURL,
			};
			if (truyen != null) truyen(dichVu);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtTenDichVu.Text))
			{
				new DialogCustoms("Vui lòng nhập tên dịch vụ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtMoTaDichVu.Text))
			{
				new DialogCustoms("Vui lòng nhập mô tả dịch vụ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(cmbMaLoai.Text))
			{
				new DialogCustoms("Vui lòng chọn mã loại dịch vụ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtGia.Text))
			{
				new DialogCustoms("Vui lòng nhập giá", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			int so;
			if (int.TryParse(txtTenDichVu.Text, out so))
			{
				new DialogCustoms("Vui lòng nhập đúng định đạng tên dịch vụ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (int.TryParse(txtGia.Text, out so) == false)
			{
				new DialogCustoms("Vui lòng nhập đúng định đạng giá", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (int.TryParse(txtSoLuong.Text, out so) == false)
			{
				new DialogCustoms("Vui lòng nhập đúng định đạng số lượng", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			return true;
		}
	}
}