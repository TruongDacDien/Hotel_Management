using System.Windows;
using System.Windows.Controls;
using DAL.DTO;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for Them_SuaKhachHang.xaml
	/// </summary>
	public partial class Them_SuaKhachHang : Window
	{
		public delegate void suaData(KhachHang nv);

		public delegate void truyenData(KhachHang nv);

		private readonly string maKH;
		public suaData suaKhachHang;


		public truyenData truyenKhachHang;

		public Them_SuaKhachHang()
		{
			InitializeComponent();
			txtCCCD.MaxLength = 12;
			txtSoDienThoai.MaxLength = 10;
		}

		public Them_SuaKhachHang(KhachHang kh) : this()
		{
			txtTenKhachHang.Text = kh.TenKH;
			cmbGioiTinh.Text = kh.GioiTinh;
			txtSoDienThoai.Text = kh.SDT;
			txtCCCD.Text = kh.CCCD;
			txtDiaChi.Text = kh.DiaChi;
			txtQuocTich.Text = kh.QuocTich;
			txbTitle.Text = "Sửa khách hàng " + kh.MaKH;
			maKH = kh.MaKH.ToString();
		}

		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var khachHang = new KhachHang
			{
				TenKH = txtTenKhachHang.Text,
				GioiTinh = cmbGioiTinh.Text,
				SDT = txtSoDienThoai.Text,
				CCCD = txtCCCD.Text,
				DiaChi = txtDiaChi.Text,
				QuocTich = txtQuocTich.Text
			};
			if (truyenKhachHang != null) truyenKhachHang(khachHang);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var khachHang = new KhachHang
			{
				MaKH = int.Parse(maKH),
				TenKH = txtTenKhachHang.Text,
				GioiTinh = cmbGioiTinh.Text,
				SDT = txtSoDienThoai.Text,
				CCCD = txtCCCD.Text,
				DiaChi = txtDiaChi.Text,
				QuocTich = txtQuocTich.Text
			};
			if (suaKhachHang != null) suaKhachHang(khachHang);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtTenKhachHang.Text))
			{
				new DialogCustoms("Vui lòng nhập tên khách hàng", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(cmbGioiTinh.Text))
			{
				new DialogCustoms("Vui lòng chọn giới tính", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtCCCD.Text))
			{
				new DialogCustoms("Vui lòng nhập mã căn cước công dân", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
			{
				new DialogCustoms("Vui lòng nhập địa chỉ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtQuocTich.Text))
			{
				new DialogCustoms("Vui lòng nhập quốc tịch", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
			{
				new DialogCustoms("Vui lòng nhập số điện thoại", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			long check;
			int so;

			if (int.TryParse(txtTenKhachHang.Text, out so))
			{
				new DialogCustoms("Vui lòng nhập đúng định dạng tên khách hàng", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (int.TryParse(txtQuocTich.Text, out so))
			{
				new DialogCustoms("Vui lòng nhập đúng định dạng quốc tịch", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (int.TryParse(txtDiaChi.Text, out so))
			{
				new DialogCustoms("Vui lòng nhập đúng định dạng địa chỉ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (txtSoDienThoai.Text.Length < 10 || int.TryParse(txtSoDienThoai.Text, out so) == false)
			{
				new DialogCustoms("Sai số điện thoại", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (txtCCCD.Text.Length > 12 || txtCCCD.Text.Length < 12 || long.TryParse(txtCCCD.Text, out check) == false)
			{
				new DialogCustoms("Sai mã căn cước công dân", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			return true;
		}
	}
}