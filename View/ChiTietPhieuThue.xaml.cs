using System.Collections.Generic;
using System.Windows;
using BUS;
using DAL.DTO;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for ChiTietPhieuThue.xaml
	/// </summary>
	public partial class ChiTietPhieuThue : Window
	{
		private readonly List<CT_PhieuThue> lsCTPT;

		public ChiTietPhieuThue()
		{
			InitializeComponent();
		}

		public ChiTietPhieuThue(PhieuThue_Custom ptct) : this()
		{
			txblTenKH.Text = ptct.TenKH;
			txblNgayLapHD.Text = ptct.NgayLapPhieu.ToString();
			txblTenNV.Text = ptct.TenNV;
			txblTieuDe.Text += ptct.MaPhieuThue.ToString();
			lsCTPT = new List<CT_PhieuThue>();
			lsCTPT = CT_PhieuThueBUS.GetInstance().getCTPhieuThueTheoMaPT(ptct.MaPhieuThue);
			lvCTPT.ItemsSource = lsCTPT;
		}

		private void click_Thoat(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}