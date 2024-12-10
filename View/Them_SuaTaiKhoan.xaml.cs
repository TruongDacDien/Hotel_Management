using DAL.DTO;
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
using System.Windows.Shapes;

namespace GUI.View
{
	/// <summary>
	/// Interaction logic for Them_SuaTaiKhoan.xaml
	/// </summary>
	public partial class Them_SuaTaiKhoan : Window
	{
		public Them_SuaTaiKhoan()
		{
			InitializeComponent();
		}

		public delegate void truyenData(TaiKhoan taiKhoan);
		public delegate void suaData(TaiKhoan taiKhoan);


		public truyenData truyenTaiKhoan;
		public suaData suaTaiKhoan;

		private string username;

		public Them_SuaTaiKhoan(TaiKhoan taiKhoan) : this()
		{
			//txtTenLoaiPhong.IsReadOnly = true;
			//txtTenLoaiPhong.Text = loaiPhong.TenLoaiPhong;
			//txtSoNguoiToiDa.Text = loaiPhong.SoNguoiToiDa.ToString();

			//txtGiaNgay.Text = loaiPhong.GiaNgay % 1 == 0 ? ((int)loaiPhong.GiaNgay).ToString() : loaiPhong.GiaNgay.ToString();
			//txtGiaGio.Text = loaiPhong.GiaGio % 1 == 0 ? ((int)loaiPhong.GiaGio).ToString() : loaiPhong.GiaGio.ToString();
			//txbTitle.Text = "Sửa thông tin " + loaiPhong.MaLoaiPhong;
			//maLoaiPhong = loaiPhong.MaLoaiPhong.ToString();
		}

		#region Method
		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtTenLoaiPhong.Text))
			{
				new DialogCustoms("Vui lòng nhập tên loại phòng", "Thông báo", DialogCustoms.OK).Show();
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
			else
			{
				int so;
				if (int.TryParse(txtTenLoaiPhong.Text, out so) == true)
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
				else
				{
					return true;
				}
			}
		}
		#endregion

		#region Event
		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{

			if (!KiemTra())
			{
				return;
			}
			else
			{
				TaiKhoan taiKhoan = new TaiKhoan()
				{
					//MaLoaiPhong = int.Parse(maLoaiPhong.ToString()),
					//TenLoaiPhong = txtTenLoaiPhong.Text,
					//SoNguoiToiDa = int.Parse(txtSoNguoiToiDa.Text),
					//GiaGio = decimal.Parse(txtGiaGio.Text),
					//GiaNgay = decimal.Parse(txtGiaNgay.Text),

				};
				if (suaTaiKhoan != null)
				{
					suaTaiKhoan(taiKhoan);
				}
			}

			Window wd = Window.GetWindow(sender as Button);
			wd.Close();
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra())
			{
				return;
			}
			else
			{
				TaiKhoan taiKhoan = new TaiKhoan()
				{
					//TenLoaiPhong = txtTenLoaiPhong.Text,
					//SoNguoiToiDa = int.Parse(txtSoNguoiToiDa.Text),
					//GiaGio = decimal.Parse(txtGiaGio.Text),
					//GiaNgay = decimal.Parse(txtGiaNgay.Text),
				};
				if (truyenTaiKhoan != null)
				{
					truyenTaiKhoan(taiKhoan);
				}
			}
			Window wd = Window.GetWindow(sender as Button);
			wd.Close();
		}
		#endregion
	}
}
