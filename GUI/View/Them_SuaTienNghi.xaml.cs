using System.Windows;
using System.Windows.Controls;
using DAL.DTO;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for Them_SuaTienNghi.xaml
	/// </summary>
	public partial class Them_SuaTienNghi : Window
	{
		public delegate void suaData(TienNghi tienNghi);

		public delegate void truyenData(TienNghi tienNghi);

		private readonly int maTN;
		public suaData suaTN;


		public truyenData truyenTN;

		public Them_SuaTienNghi()
		{
			InitializeComponent();
		}

		public Them_SuaTienNghi(TienNghi tn) : this()
		{
			txtTenTN.Text = tn.TenTN;
			txtSoLuong.Text = tn.SoLuong.ToString();
			txbTitle.Text = "Sửa thông tin " + tn.MaTN;
			maTN = tn.MaTN;
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var tienNghi = new TienNghi
			{
				TenTN = txtTenTN.Text,
				SoLuong = int.Parse(txtSoLuong.Text),
			};
			if (truyenTN != null) truyenTN(tienNghi);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var tienNghi = new TienNghi
			{
				MaTN = maTN,
				TenTN = txtTenTN.Text,
				SoLuong = int.Parse(txtSoLuong.Text),
			};
			if (suaTN != null) suaTN(tienNghi);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private bool KiemTra()
		{
			// Kiểm tra txtTenTN
			if (string.IsNullOrWhiteSpace(txtTenTN.Text))
			{
				new DialogCustoms("Vui lòng nhập tên đầy đủ", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			// Kiểm tra txtSoLuong
			if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
			{
				new DialogCustoms("Vui lòng nhập số lượng", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			// Kiểm tra xem txtSoLuong có phải là số nguyên dương hợp lệ hay không
			if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
			{
				new DialogCustoms("Số lượng phải là một số nguyên dương", "Thông báo", DialogCustoms.OK).ShowDialog();
				return false;
			}

			return true;
		}
	}
}