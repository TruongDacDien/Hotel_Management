using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BUS;
using DAL.DTO;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for Them_SuaPhong.xaml
	/// </summary>
	public partial class Them_SuaPhong : Window
	{
		public delegate void SuaDuLieu(Phong p);

		public delegate void TryenDuLieu(Phong p);

		private readonly List<LoaiPhong> LP;
		private string soPhong;
		public SuaDuLieu sua;

		public TryenDuLieu truyen;

		public Them_SuaPhong()
		{
			InitializeComponent();
			LP = new List<LoaiPhong>(LoaiPhongBUS.Instance.getDataLoaiPhong());
			cmbLoaiPhong.ItemsSource = LP;
			cmbLoaiPhong.DisplayMemberPath = "TenLoaiPhong";
			cmbLoaiPhong.SelectedValuePath = "MaLoaiPhong";
		}

		public Them_SuaPhong(Phong phong) : this()
		{
			cmbLoaiPhong.DisplayMemberPath = "TenLoaiPhong";
			cmbLoaiPhong.SelectedValuePath = "MaLoaiPhong";

			txtSoPhong.IsReadOnly = true;
			txtSoPhong.Text = phong.SoPhong;
			cmbTinhTrang.Text = phong.DonDep;
			cmbLoaiPhong.Text = phong.LoaiPhong;
			txbTitle.Text = "Sửa thông tin phòng " + phong.SoPhong;
			soPhong = phong.SoPhong;
		}

		#region Method

		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtSoPhong.Text))
			{
				new DialogCustoms("Vui lòng nhập số phòng", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(cmbLoaiPhong.Text))
			{
				new DialogCustoms("Vui lòng chọn loại phòng", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(cmbTinhTrang.Text))
			{
				new DialogCustoms("Vui lòng chọn tình trạng", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			int so;
			if (int.TryParse(txtSoPhong.Text, out so) || KiemTraTenPhong() == false)
			{
				new DialogCustoms("Vui lòng nhập đúng định dạng số phòng ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			return true;
		}

		public bool KiemTraTenPhong()
		{
			var str = txtSoPhong.Text;
			int so;
			if (str[0].Equals('P') && int.TryParse(str[1].ToString(), out so) &&
			    int.TryParse(str[2].ToString(), out so) && int.TryParse(str[3].ToString(), out so)) return true;

			return false;
		}

		#endregion

		#region Event

		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var phong = new Phong
			{
				SoPhong = txtSoPhong.Text,
				DonDep = cmbTinhTrang.Text,
				MaLoaiPhong = (int)cmbLoaiPhong.SelectedValue
			};
			if (truyen != null) truyen(phong);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var phong = new Phong

			{
				SoPhong = txtSoPhong.Text,
				DonDep = cmbTinhTrang.Text,
				MaLoaiPhong = (int)cmbLoaiPhong.SelectedValue
			};
			if (sua != null) sua(phong);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		#endregion
	}
}