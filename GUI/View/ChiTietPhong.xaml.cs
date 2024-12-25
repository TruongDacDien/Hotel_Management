using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BUS;
using DAL.DTO;
using MaterialDesignThemes.Wpf;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for ChiTietPhong.xaml
	/// </summary>
	public partial class ChiTietPhong : Window
	{
		public delegate void truyenDataPhong(Phong_Custom phong);

		private bool kiemTraNhanPhong;
		private bool kiemTraSuaDoiTinhTrangDonDep;
		private int? maCTPhieuThue;

		private ObservableCollection<DichVu_DaChon> obDichVu;
		private Phong_Custom phong_CTPhong;
		public truyenDataPhong truyenData;

		public ChiTietPhong()
		{
			InitializeComponent();
			truyenData = setDataPhongCustom;
			kiemTraSuaDoiTinhTrangDonDep = false;
			kiemTraNhanPhong = false;
		}

		public ChiTietPhong(int maNV) : this()
		{
			MaNV = maNV;
		}

		public int MaNV { get; set; }

		#region method

		private void setDataPhongCustom(Phong_Custom phong)
		{
			//Nhận dữ liệu từ form cha và gán giá trị lên form con
			phong_CTPhong = phong;
			txblTieuDe.Text = "Thông tin chi tiết phòng " + phong.MaPhong;
			txblTenKH.Text = phong.TenKH;
			if (phong.IsDay)
			{
				icDayorHour.Kind = PackIconKind.CalendarToday;
				txblSoNgay.Text = phong.SoNgayO + "  ngày";
			}
			else
			{
				icDayorHour.Kind = PackIconKind.AlarmCheck;
				txblSoNgay.Text = phong.SoGio + "  giờ";
			}

			txblSoNguoi.Text = phong.SoNguoi.ToString();
			txblNgayDen.Text = phong.NgayDen.ToString();
			cbTinhTrang.Text = phong.TinhTrang;
			cbDonDep.Text = phong.DonDep;
			kiemTraSuaDoiTinhTrangDonDep = false;
			//Lấy ra mã CT phiếu thuê
			maCTPhieuThue = phong.MaCTPT;
			//Lấy chi tiết sử dụng dịch vụ của phòng đó nếu có
			if (maCTPhieuThue != null)
				obDichVu = new ObservableCollection<DichVu_DaChon>(CTSDDV_BUS.GetInstance()
					.getCTSDDVtheoMaCTPT(maCTPhieuThue));
			else
				obDichVu = new ObservableCollection<DichVu_DaChon>();

			lvSuDungDV.ItemsSource = obDichVu;
		}

		private void nhanData(ObservableCollection<DichVu_DaChon> obDVCT)
		{
			foreach (var item in obDVCT) obDichVu.Add(item);
		}

		#endregion

		#region event

		private void click_Thoat(object sender, RoutedEventArgs e)
		{
			Close();
		}


		private void click_NhanPhong(object sender, RoutedEventArgs e)
		{
			kiemTraNhanPhong = true;
			cbTinhTrang.Text = "Phòng đang thuê";
		}

		private void click_ThanhToan(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Visibility = Visibility.Hidden;
			var hoaDon = new XuatHoaDon(MaNV, phong_CTPhong, obDichVu);
			hoaDon.ShowDialog();
			Close();
		}

		private void click_ThemDV(object sender, RoutedEventArgs e)
		{
			var cTP_ThemDV = new CTP_ThemDV(maCTPhieuThue);
			cTP_ThemDV.truyenData = nhanData;
			cTP_ThemDV.ShowDialog();
		}

		private void cbDonDep_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			kiemTraSuaDoiTinhTrangDonDep = true;
		}

		private void click_Luu(object sender, RoutedEventArgs e)
		{
			if (kiemTraSuaDoiTinhTrangDonDep)
			{
				var error = string.Empty;
				if (!PhongBUS.GetInstance().suaTinhTrangDonDep(phong_CTPhong.MaPhong, cbDonDep.Text, out error))
				{
					new DialogCustoms("Lưu thất bại !\n Lỗi:" + error, "Thông báo", DialogCustoms.OK).ShowDialog();
					return;
				}

				DialogResult = true;
				Close();
			}

			if (kiemTraNhanPhong)
			{
				var error = string.Empty;
				if (!CT_PhieuThueBUS.GetInstance()
					    .suaTinhTrangThuePhong(phong_CTPhong.MaCTPT, "Phòng đang thuê", out error))
				{
					new DialogCustoms("Lưu thất bại !\n Lỗi:" + error, "Thông báo", DialogCustoms.OK).ShowDialog();
					return;
				}

				CT_PhieuThueBUS.GetInstance().capNhatNgayBD(phong_CTPhong.MaCTPT, DateTime.Now, out error);
				DialogResult = true;
				Close();
			}

			if (!kiemTraSuaDoiTinhTrangDonDep && !kiemTraNhanPhong) Close();
		}

		#endregion
	}
}