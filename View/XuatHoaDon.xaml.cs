using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DAL.DTO;
using BUS;
using DAL.Data;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for XuatHoaDon.xaml
    /// </summary>
    public partial class XuatHoaDon : Window
    {
        private Phong_Custom phong;
        private int maNV;
        private List<DichVu_DaChon> ls;
        public Phong_Custom Phong { get => phong; set => phong = value; }
        public int MaNV { get => maNV; set => maNV = value; }

        public XuatHoaDon()
        {
            InitializeComponent();
        }
		public XuatHoaDon(int maNV, Phong_Custom ph, ObservableCollection<DichVu_DaChon> lsDV) : this()
		{
			this.MaNV = maNV;
			this.Phong = ph;
			ls = lsDV.ToList();
			try
			{
				string error = string.Empty;

				// Cập nhật thời gian kết thúc thuê phòng
				DateTime ngayKT = DateTime.Now;
				CT_PhieuThueBUS.GetInstance().capNhatNgayKT(Phong.MaCTPT, ngayKT, out error);

				// Lấy thông tin chi tiết phiếu thuê
				CT_PhieuThue cT_PhieuThue = CT_PhieuThueBUS.GetInstance().getCTPhieuThueTheoMaCTPT(Phong.MaCTPT);

				// Tính tiền phòng và tiền dịch vụ
				decimal? tienPhong = PhongBUS.GetInstance().tinhTienPhong(Phong, cT_PhieuThue);
				decimal? tienDV = CTSDDV_BUS.GetInstance().tinhTongTienDichVuTheoMaCTPT(Phong.MaCTPT);

				// Hiển thị thông tin chi tiết
				txbSoPhong.Text = Phong.MaPhong;

				// Phân tích số ngày và giờ từ tổng thời gian thuê
				TimeSpan time = ngayKT - cT_PhieuThue.NgayBD;
				int songay = (int)time.TotalDays;
				int sogio = time.Hours + (time.Seconds > 0 ? 1 : 0); // Làm tròn giờ nếu có phút dư

				string thoiGianThue = $"{songay} ngày {sogio} giờ";

				// Hiển thị thời gian thuê
				txbSoNgayOrGio.Text = "Thời gian thuê: ";
				txbSoNgay.Text = thoiGianThue;

				txbTenKH.Text = "Khách hàng: " + Phong.TenKH;
				txbSoNguoi.Text = Phong.SoNguoi.ToString();
				txbNhanVien.Text = NhanVienBUS.GetInstance().layNhanVienTheoMaNV(MaNV);
				txbNgayLapHD.Text = ngayKT.ToString();
				txbTongTien.Text = string.Format("{0:0,0 VND}", ((tienDV ?? 0) + tienPhong));

				// Thêm hóa đơn vào DB
				HoaDon hd = new HoaDon()
				{
					MaNV = this.MaNV,
					MaCTPT = Phong.MaCTPT,
					NgayLap = ngayKT,
					TongTien = decimal.Parse(((tienDV ?? 0) + tienPhong).ToString())
				};

				if (!HoaDonBUS.GetInstance().themHoaDon(hd, out error))
				{
					new DialogCustoms("Thêm hóa đơn thất bại!\nLỗi: " + error, "Thông báo", DialogCustoms.OK).ShowDialog();
				}

				// Lấy mã hóa đơn mới nhất
				txbSoHoaDon.Text = HoaDonBUS.GetInstance().layMaHDMoiNhat().ToString();

				// Sửa trạng thái của phiếu thuê
				string errorSuaCTPT = string.Empty;
				if (!CT_PhieuThueBUS.GetInstance().suaTinhTrangThuePhong(Phong.MaCTPT, "Đã thanh toán", out errorSuaCTPT))
				{
					new DialogCustoms("Lỗi sửa CTPT\nLỗi: " + errorSuaCTPT, "Thông báo", DialogCustoms.OK).ShowDialog();
				}

				// Cập nhật tiền phòng cho phiếu thuê
				string errorCapNhatCTPT = string.Empty;
				if (!CT_PhieuThueBUS.GetInstance().capNhatTien(ph.MaCTPT, tienPhong, out errorCapNhatCTPT))
				{
					new DialogCustoms("Lỗi cập nhật CTPT\nLỗi: " + errorCapNhatCTPT, "Thông báo", DialogCustoms.OK).ShowDialog();
				}

				// Thêm dịch vụ thuê phòng vào danh sách dịch vụ đã sử dụng
				decimal temp = 0;
				if (songay > 0) temp = decimal.Parse($"{songay}.{sogio:D2}"); else temp = sogio;
				DichVu_DaChon dv = new DichVu_DaChon()
				{
					SoLuong = temp, // Hiển thị số lượng theo định dạng
					TenDV = "Thuê phòng",
					Gia = PhongBUS.GetInstance().layTienPhongTheoSoPhong(Phong),
					ThanhTien = tienPhong
				};

				ls.Add(dv);
				lvDichVuDaSD.ItemsSource = ls;
			}
			catch (Exception ex)
			{
				new DialogCustoms("Lỗi load thông tin!\nLỗi: " + ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
			}
		}

		public XuatHoaDon(HoaDon hoaDon) : this()
		{
			try
			{
				txbNhanVien.Text = hoaDon.NhanVien.HoTen.ToString();
				txbSoPhong.Text = hoaDon.CT_PhieuThue.SoPhong.ToString();

				DateTime ngayBD = hoaDon.CT_PhieuThue.NgayBD;
				DateTime ngayKT = hoaDon.CT_PhieuThue.NgayKT;

				// Phân tích số ngày và giờ
				TimeSpan time = ngayKT - ngayBD;
				int songay = (int)time.TotalDays;
				int sogio = time.Hours + (time.Seconds > 0 ? 1 : 0);

				string thoiGianThue = $"{songay} ngày {sogio} giờ";

				// Hiển thị thời gian thuê
				txbSoNgayOrGio.Text = "Thời gian thuê: ";
				txbSoNgay.Text = thoiGianThue;

				txbSoHoaDon.Text = hoaDon.MaHD.ToString();
				txbTenKH.Text = "Khách hàng: " + KhachHangBUS.GetInstance().layTenKhachHangTheoMaPT(hoaDon.CT_PhieuThue.MaPhieuThue);
				txbSoNguoi.Text = hoaDon.CT_PhieuThue.SoNguoiO.ToString();
				txbNgayLapHD.Text = hoaDon.NgayLap.ToString();
				txbTongTien.Text = string.Format("{0:0,0 VND}", hoaDon.TongTien);

				// Danh sách dịch vụ đã sử dụng
				ls = new List<DichVu_DaChon>(CTSDDV_BUS.GetInstance().getCTSDDVtheoMaCTPT(hoaDon.MaCTPT));
				decimal temp = 0;
				if (songay > 0) temp = decimal.Parse($"{songay}.{sogio:D2}"); else temp = sogio;
				DichVu_DaChon dv = new DichVu_DaChon()
				{
					SoLuong = temp,
					TenDV = "Thuê phòng",
					Gia = PhongBUS.GetInstance().layTienPhongTheoSoPhong(hoaDon.CT_PhieuThue.SoPhong, songay > 0),
					ThanhTien = hoaDon.CT_PhieuThue.TienPhong
				};
				ls.Add(dv);
				lvDichVuDaSD.ItemsSource = ls;
			}
			catch (Exception ex)
			{
				new DialogCustoms("Lỗi: " + ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
			}
		}

		private void click_InHoaDon(object sender, RoutedEventArgs e)
        {
            try
            {
                btnInHoaDon.Visibility = Visibility.Hidden;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "In Hóa đơn");
                    new DialogCustoms("In hóa đơn thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
                }
            }
            catch(Exception ex)
            {
                new DialogCustoms("In hóa đơn thất bại! \n Lỗi: "+ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
            }
            finally
            {
                btnInHoaDon.Visibility = Visibility.Visible;
            }
        }   
    }
}
