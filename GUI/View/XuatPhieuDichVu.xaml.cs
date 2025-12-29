using System;
using System.Windows;
using System.Windows.Controls;
using DAL.DTO;

namespace GUI.View
{
	/// <summary>
	/// Interaction logic for XuatPhieuDichVu.xaml
	/// </summary>
	public partial class XuatPhieuDichVu : Window
	{
		private readonly CT_SDDichVu ct;

		public XuatPhieuDichVu(CT_SDDichVu ct)
		{
			InitializeComponent();
			this.ct = ct;
			LoadData();
		}

		private void LoadData()
		{
			txbMaPhieu.Text = ct.MaCTSDDV.ToString();
			txbTenKH.Text = ct.TenKH ?? string.Empty;
			txbSoPhong.Text = ct.SoPhong ?? "N/A";
			txbTenDV.Text = ct.TenDV ?? string.Empty;
			txbSoLuong.Text = ct.SL.ToString();
			txbNgayApDung.Text = ct.NgayApDung.ToString("dd/MM/yyyy HH:mm");
			txbThanhTien.Text = string.Format("{0:0,0 VND}", ct.ThanhTien);
		}

		private void click_InPhieu(object sender, RoutedEventArgs e)
		{
			try
			{
				var printDialog = new PrintDialog();
				if (printDialog.ShowDialog() == true)
				{
					printDialog.PrintVisual(printArea, "In phieu dich vu");
					new DialogCustoms("In phieu thanh cong!", "Thong bao", DialogCustoms.OK).ShowDialog();
				}
			}
			catch (Exception ex)
			{
				new DialogCustoms("In phieu that bai!\nLoi: " + ex.Message, "Thong bao", DialogCustoms.OK)
					.ShowDialog();
			}
		}

		private void click_Thoat(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
