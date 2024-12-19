using BUS;
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
	/// Interaction logic for MW_ThongTinCaNhan.xaml
	/// </summary>
	public partial class MW_ThongTinCaNhan : Window
	{
		private  NhanVien nhanVien;
		public NhanVien NhanVien { get => nhanVien; set => nhanVien = value; }

		public MW_ThongTinCaNhan()
		{
			InitializeComponent();
			txbLuong.IsReadOnly = true;
			txbChucVu.IsReadOnly = true;
		}

		public MW_ThongTinCaNhan(int maNV): this()
		{
			this.NhanVien = NhanVienBUS.GetInstance().layNhanVienTheoMaNV(maNV);
			if (this.NhanVien != null)
			{
				txbHoTenNV.Text = this.NhanVien.HoTen;
				txbCCCD.Text = this.NhanVien.CCCD;
				txbChucVu.Text = this.NhanVien.ChucVu;
				txbDiaChi.Text = this.NhanVien.DiaChi;
				cbGioiTinh.ItemsSource = new List<string> { "Nam", "Nữ" };
				cbGioiTinh.SelectedValue = this.NhanVien.GioiTinh ?? "Nam";
				txbLuong.Text = this.NhanVien.Luong % 1 == 0
				? ((int)this.NhanVien.Luong).ToString()
				: this.NhanVien.Luong.ToString();
				dtNTNS.SelectedDate = this.NhanVien.NTNS;
				txbSDT.Text = this.NhanVien.SDT;
			}	
		}

		private bool kiemTraDayDuThongTin()
		{
			if (string.IsNullOrWhiteSpace(txbHoTenNV.Text))
			{
				txbHoTenNV.Focus();
				new DialogCustoms("Nhập đầy đủ họ tên !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			// Ngày tháng năm sinh
			if (string.IsNullOrWhiteSpace(dtNTNS.Text))
			{
				dtNTNS.Focus();
				new DialogCustoms("Nhập đúng định dạng ngày tháng năm sinh !", "Thông báo", DialogCustoms.OK)
					.ShowDialog();
				return false;
			}

			if (!DateTime.TryParse(dtNTNS.Text, out var dt))
			{
				dtNTNS.Focus();
				new DialogCustoms("Nhập đúng định dạng ngày tháng năm sinh !", "Thông báo", DialogCustoms.OK)
					.ShowDialog();
				return false;
			}

			//Căn cước công dân
			if (string.IsNullOrWhiteSpace(txbCCCD.Text))
			{
				txbCCCD.Focus();
				new DialogCustoms("Nhập đầy đủ căn cước công dân !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			if (!long.TryParse(txbCCCD.Text, out var cccd))
			{
				txbCCCD.Focus();
				new DialogCustoms("Nhập căn cước công dân đúng định dạng số !", "Thông báo", DialogCustoms.OK)
					.ShowDialog();
				return false;
			}

			if (txbCCCD.Text.Length > 12)
			{
				txbCCCD.Focus();
				new DialogCustoms("Nhập căn cước công dân không quá 12 ký tự !", "Thông báo", DialogCustoms.OK)
					.ShowDialog();
				return false;
			}

			// Lương
			if (string.IsNullOrWhiteSpace(txbLuong.Text))
			{
				txbLuong.Focus();
				new DialogCustoms("Nhập đầy đủ lương !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			if (!decimal.TryParse(txbLuong.Text, out var luong))
			{
				txbLuong.Focus();
				new DialogCustoms("Nhập lương đúng định dạng số !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			// Số điện thoại
			if (string.IsNullOrWhiteSpace(txbSDT.Text))
			{
				txbSDT.Focus();
				new DialogCustoms("Nhập đầy đủ số điện thoại !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			if (!long.TryParse(txbSDT.Text, out var sdt))
			{
				txbSDT.Focus();
				new DialogCustoms("Nhập số điện thoại đúng định dạng số !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			if (txbSDT.Text.Length > 10)
			{
				txbSDT.Focus();
				new DialogCustoms("Nhập số điện thoại không quá 10 ký tự !", "Thông báo", DialogCustoms.OK)
					.ShowDialog();
				return false;
			}

			// Giới tính
			if (string.IsNullOrWhiteSpace(cbGioiTinh.Text))
			{
				cbGioiTinh.Focus();
				new DialogCustoms("Nhập đầy đủ giới tính !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			//Địa chỉ
			if (string.IsNullOrWhiteSpace(txbDiaChi.Text))
			{
				txbDiaChi.Focus();
				new DialogCustoms("Nhập đầy đủ địa chỉ !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			// Chức vụ
			if (string.IsNullOrWhiteSpace(txbChucVu.Text))
			{
				txbChucVu.Focus();
				new DialogCustoms("Nhập đầy đủ chức vụ !", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			//nếu đúng hết thì trả về false
			return true;
		}

		private void click_CapNhat(object sender, RoutedEventArgs e)
		{
			if (kiemTraDayDuThongTin())
			{
				var nhanVien = new NhanVien
				{
					MaNV = this.NhanVien.MaNV,
					HoTen = txbHoTenNV.Text,
					CCCD = txbCCCD.Text,
					ChucVu = txbChucVu.Text,
					DiaChi = txbDiaChi.Text,
					GioiTinh = cbGioiTinh.Text,
					Luong = decimal.Parse(txbLuong.Text),
					NTNS = DateTime.Parse(dtNTNS.SelectedDate.ToString()),
					SDT = txbSDT.Text
				};
				if (NhanVienBUS.GetInstance().updateNhanVien(nhanVien))
				{
					new DialogCustoms("Cập nhật thông tin cá nhân thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
				}
			}
			Close();
		}

		private void click_Huy(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
