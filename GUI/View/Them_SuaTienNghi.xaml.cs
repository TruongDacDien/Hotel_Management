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
			txbTitle.Text = "Sửa thông tin " + tn.MaTN;
			maTN = tn.MaTN;
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var tienNghi = new TienNghi
			{
				MaTN = maTN,
				TenTN = txtTenTN.Text
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
				TenTN = txtTenTN.Text
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
			if (string.IsNullOrWhiteSpace(txtTenTN.Text))
			{
				MessageBox.Show("Vui lòng nhập đầy dủ thông tin");
				return false;
			}

			int so;
			if (int.TryParse(txtTenTN.Text, out so))
			{
				MessageBox.Show("Vui lòng nhập đúng dữ liệu");
				return false;
			}

			return true;
		}
	}
}