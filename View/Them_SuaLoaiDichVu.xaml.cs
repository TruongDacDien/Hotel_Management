using System.Windows;
using System.Windows.Controls;
using DAL.DTO;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for Them_SuaLoaiDichVu.xaml
	/// </summary>
	public partial class Them_SuaLoaiDichVu : Window
	{
		public delegate void suaData(LoaiDV loaiDV);

		public delegate void truyenData(LoaiDV loaiDV);

		private readonly string maLoai;
		public suaData suaLoaiDV;


		public truyenData truyenLoaiDV;

		public Them_SuaLoaiDichVu()
		{
			InitializeComponent();
		}

		public Them_SuaLoaiDichVu(LoaiDV loaiDV) : this()
		{
			txtTenLoaiDV.Text = loaiDV.TenLoaiDV;
			txbTitle.Text = "Sửa thông tin" + loaiDV.MaLoaiDV;

			maLoai = loaiDV.MaLoaiDV.ToString();
		}

		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var loaiDV = new LoaiDV
			{
				TenLoaiDV = txtTenLoaiDV.Text
			};
			if (truyenLoaiDV != null) truyenLoaiDV(loaiDV);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var loaiDV = new LoaiDV
			{
				MaLoaiDV = int.Parse(maLoai),
				TenLoaiDV = txtTenLoaiDV.Text
			};
			if (suaLoaiDV != null) suaLoaiDV(loaiDV);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtTenLoaiDV.Text))
			{
				new DialogCustoms("Vui lòng nhập tên loại dịch vụ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			int so;
			if (int.TryParse(txtTenLoaiDV.Text, out so))
			{
				new DialogCustoms("Vui lòng nhập đúng định dạng tên loại dịch vụ", "Thông báo", DialogCustoms.OK)
					.Show();
				return false;
			}

			return true;
		}
	}
}