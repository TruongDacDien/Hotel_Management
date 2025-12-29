using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BUS;
using DAL.DTO;
using Net.payOS;
using Net.payOS.Types;
using System.Diagnostics;
using DocumentFormat.OpenXml.Drawing;
using System.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Windows.Media;
using DAL.Data;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Windows.Media.Animation;
using GUI.UserControls;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for XuatHoaDon.xaml
	/// </summary>
	public partial class XuatHoaDon : Window
	{
		private readonly List<DichVu_DaChon> ls;
		private CT_SDDichVu? ctSDDV;
		private bool updateTrangThaiOnPrint;
		private PayOS payOS { get; set; }

		private DateTime ngayKT { get; set; }

		public Phong_Custom Phong { get; set; }

		public int MaNV { get; set; }

		public XuatHoaDon()
		{
			InitializeComponent();
			string clientId = Properties.Resources.clientId;
			string apiKey = Properties.Resources.apiKey;
			string checksumKey = Properties.Resources.checksumKey;
			payOS = new PayOS(clientId, apiKey, checksumKey);
		}

		public XuatHoaDon(int maNV, CT_SDDichVu ct, bool allowPayment, bool allowPrint, bool capNhatTrangThaiSauIn) : this()
		{
			MaNV = maNV;
			ctSDDV = ct;
			updateTrangThaiOnPrint = capNhatTrangThaiSauIn;

			ls = new List<DichVu_DaChon>();
			ConfigureButtons(allowPayment, allowPrint);
			LoadThongTinDichVu(ct, false);
		}

		public XuatHoaDon(int maNV, Phong_Custom ph, ObservableCollection<DichVu_DaChon> lsDV) : this()
		{
			MaNV = maNV;
			Phong = ph;
			ls = lsDV.ToList();

			try
			{
				var error = string.Empty;

				// Lấy thời gian kết thúc thuê phòng là hiện tại
				ngayKT = DateTime.Now;

				// Lấy thông tin chi tiết phiếu thuê
				var cT_PhieuThue = CT_PhieuThueBUS.GetInstance().getCTPhieuThueTheoMaCTPT(Phong.MaCTPT);

				// Tính tiền phòng và dịch vụ
				var tienDV = CTSDDV_BUS.GetInstance().tinhTongTienDichVuTheoMaCTPT(Phong.MaCTPT);
				decimal tienPhong;

				// Hiển thị thông tin chi tiết
				txbSoPhong.Text = Phong.MaPhong;

				// Phân tích ngày và giờ
				var time = ngayKT - cT_PhieuThue.NgayBD;
				var songay = (int)time.TotalDays; // Số ngày đã thuê

				// Tính số giờ
				decimal sogio = (decimal)time.TotalHours;

				// Làm tròn giờ: nếu số giờ dư trong ngày > 12 giờ thì tính thêm 1 ngày
				decimal gioDu = (decimal)(time.TotalHours % 24);
				if (gioDu > 12)
				{
					songay++;     // Trên 12 giờ: tính thêm 1 ngày
					sogio = 0;    // Sau khi tính thêm 1 ngày thì không cần tính giờ nữa
				}
				else
				{
					// Trường hợp còn lại: làm tròn số giờ thuê theo yêu cầu
					sogio = sogio == 0 ? 1 : (time.Hours + (time.Minutes > 30 || time.Seconds > 0 ? 1 : 0));
				}

				var thoiGianThue = $"{songay} ngày {sogio} giờ";

				var giaNgay = PhongDAL.GetInstance().layGiaTienTheoMaPhong(Phong.MaPhong, true);
				var giaGio = PhongDAL.GetInstance().layGiaTienTheoMaPhong(Phong.MaPhong, false);
				tienPhong = (songay > 0 ? giaNgay * songay : 0) + (sogio > 0 ? giaGio * sogio : 0);

				// Hiển thị thời gian thuê
				txbSoNgayOrGio.Text = "Thời gian thuê: ";
				txbSoNgay.Text = thoiGianThue;

				txbTenKH.Text = "Khách hàng: " + Phong.TenKH;
				txbSoNguoi.Text = Phong.SoNguoi.ToString();
				txbNhanVien.Text = NhanVienBUS.GetInstance().layNhanVienTheoMaNV(MaNV).HoTen;
				txbNgayLapHD.Text = ngayKT.ToString();
				txbTongTien.Text = string.Format("{0:0,0 VND}", (tienDV ?? 0) + tienPhong);

				var hoaDonTemp = HoaDonBUS.GetInstance().LayHoaDonThepMaCTPT(Phong.MaCTPT);
				if (hoaDonTemp == null)
				{
					// Thêm hóa đơn vào DB
					var hd = new HoaDon
					{
						MaNV = MaNV,
						MaCTPT = Phong.MaCTPT,
						NgayLap = ngayKT,
						TongTien = decimal.Parse(((tienDV ?? 0) + tienPhong).ToString())
					};

					if (!HoaDonBUS.GetInstance().themHoaDon(hd, out error))
					{
						new DialogCustoms("Thêm hóa đơn thất bại!\nLỗi: " + error, "Thông báo", DialogCustoms.OK)
							.ShowDialog();
					}

					// Lấy mã hóa đơn mới nhất
					txbSoHoaDon.Text = HoaDonBUS.GetInstance().layMaHDMoiNhat().ToString();

					// Cập nhật tiền phòng cho phiếu thuê
					var errorCapNhatCTPT = string.Empty;
					if (!CT_PhieuThueBUS.GetInstance().capNhatTien(ph.MaCTPT, tienPhong, out errorCapNhatCTPT))
					{
						new DialogCustoms("Lỗi cập nhật CTPT\nLỗi: " + errorCapNhatCTPT, "Thông báo", DialogCustoms.OK)
							.ShowDialog();
					}
				}
				else
				{
					txbSoHoaDon.Text = hoaDonTemp.MaHD.ToString();
					hoaDonTemp.MaNV = MaNV;
					hoaDonTemp.NgayLap = ngayKT;
					hoaDonTemp.TongTien = decimal.Parse(((tienDV ?? 0) + tienPhong).ToString());

					if (!HoaDonBUS.GetInstance().capNhatHoaDon(hoaDonTemp))
					{
						new DialogCustoms("Cập nhật hóa đơn thất bại!\nLỗi: " + error, "Thông báo", DialogCustoms.OK)
							.ShowDialog();
					}

					// Cập nhật tiền phòng cho phiếu thuê
					var errorCapNhatCTPT = string.Empty;
					if (!CT_PhieuThueBUS.GetInstance().capNhatTien(hoaDonTemp.MaCTPT, tienPhong, out errorCapNhatCTPT))
					{
						new DialogCustoms("Lỗi cập nhật CTPT\nLỗi: " + errorCapNhatCTPT, "Thông báo", DialogCustoms.OK)
							.ShowDialog();
					}
				}

				var errorCapNhatSDDV = string.Empty;
				if (!CTSDDV_BUS.GetInstance().capNhatThanhToanTheoMaCTPT(Phong.MaCTPT, true, "Direct",
					    out errorCapNhatSDDV))
				{
					Console.WriteLine(errorCapNhatSDDV);
				}

				// Thêm dịch vụ thuê phòng (theo ngày) vào danh sách dịch vụ đã sử dụng
				if (songay > 0)
				{
					var dvNgay = new DichVu_DaChon
					{
						SoLuong = songay,
						TenDV = "Thuê phòng (theo ngày)",
						Gia = PhongDAL.GetInstance().layGiaTienTheoMaPhong(Phong.MaPhong, true),
						ThanhTien = PhongDAL.GetInstance().layGiaTienTheoMaPhong(Phong.MaPhong, true) * songay
					};
					ls.Add(dvNgay);
				}

				// Thêm dịch vụ thuê phòng (theo giờ) vào danh sách dịch vụ đã sử dụng
				if (sogio > 0)
				{
					var dvGio = new DichVu_DaChon
					{
						SoLuong = sogio,
						TenDV = "Thuê phòng (theo giờ)",
						Gia = PhongDAL.GetInstance().layGiaTienTheoMaPhong(Phong.MaPhong, false),
						ThanhTien = PhongDAL.GetInstance().layGiaTienTheoMaPhong(Phong.MaPhong, false) * sogio
					};
					ls.Add(dvGio);
				}

				// Thêm danh sách dịch vụ khác
				lvDichVuDaSD.ItemsSource = ls;

				var chuaDonDep = new Phong
				{
					SoPhong = ph.MaPhong,
					DonDep = "Chưa dọn dẹp",
				};
				PhongBUS.GetInstance().capNhatDataPhong(chuaDonDep);
			}
			catch (Exception ex)
			{
				new DialogCustoms("Lỗi tải thông tin!\nLỗi: " + ex.Message, "Thông báo", DialogCustoms.OK)
					.ShowDialog();
			}
		}

		public XuatHoaDon(HoaDon hoaDon) : this()
		{
			try
			{
				groupButton.Visibility = Visibility.Hidden;
				ls = new List<DichVu_DaChon>();
				if (hoaDon == null)
				{
					new DialogCustoms("Khong tim thay hoa don.", "Thong bao", DialogCustoms.OK).ShowDialog();
					return;
				}

				txbSoHoaDon.Text = hoaDon.MaHD.ToString();
				txbNhanVien.Text = hoaDon.NhanVien?.HoTen ?? "N/A";
				txbNgayLapHD.Text = hoaDon.NgayLap.ToString();
				txbTongTien.Text = string.Format("{0:0,0 VND}", hoaDon.TongTien);

				if (hoaDon.MaCTPT.HasValue && hoaDon.CT_PhieuThue != null)
				{
					txbSoPhong.Text = hoaDon.CT_PhieuThue.SoPhong ?? "N/A";

					var ngayBD = hoaDon.CT_PhieuThue.NgayBD;
					var ngayKT = hoaDon.CT_PhieuThue.NgayKT;

					var time = ngayKT - ngayBD;
					var songay = (int)time.TotalDays;
					decimal sogio = (decimal)time.TotalHours;

					decimal gioDu = (decimal)(time.TotalHours % 24);
					if (gioDu > 12)
					{
						songay++;
						sogio = 0;
					}
					else
					{
						sogio = sogio == 0 ? 1 : (time.Hours + (time.Minutes > 30 || time.Seconds > 0 ? 1 : 0));
					}

					var thoiGianThue = $"{songay} ngày {sogio} giờ";
					txbSoNgayOrGio.Text = "Thời gian thuê: ";
					txbSoNgay.Text = thoiGianThue;

					var tenKh = KhachHangBUS.GetInstance().layTenKhachHangTheoMaPT(hoaDon.CT_PhieuThue.MaPhieuThue);
					txbTenKH.Text = "Khách hàng: " + (string.IsNullOrWhiteSpace(tenKh) ? "N/A" : tenKh);
					txbSoNguoi.Text = hoaDon.CT_PhieuThue.SoNguoiO.ToString();

					ls = new List<DichVu_DaChon>(CTSDDV_BUS.GetInstance().getCTSDDVtheoMaCTPT(hoaDon.MaCTPT));

					if (songay > 0)
					{
						var dvNgay = new DichVu_DaChon
						{
							SoLuong = songay,
							TenDV = "Thuê phòng (theo ngày)",
							Gia = PhongDAL.GetInstance().layGiaTienTheoMaPhong(hoaDon.CT_PhieuThue.SoPhong, true),
							ThanhTien = PhongDAL.GetInstance().layGiaTienTheoMaPhong(hoaDon.CT_PhieuThue.SoPhong, true) * songay
						};
						ls.Add(dvNgay);
					}

					if (sogio > 0)
					{
						var dvGio = new DichVu_DaChon
						{
							SoLuong = sogio,
							TenDV = "Thuê phòng (theo giờ)",
							Gia = PhongDAL.GetInstance().layGiaTienTheoMaPhong(hoaDon.CT_PhieuThue.SoPhong, false),
							ThanhTien = PhongDAL.GetInstance().layGiaTienTheoMaPhong(hoaDon.CT_PhieuThue.SoPhong, false) * sogio
						};
						ls.Add(dvGio);
					}

					lvDichVuDaSD.ItemsSource = ls;
					return;
				}

				var ct = hoaDon.MaCTSDDV.HasValue
					? CTSDDV_BUS.GetInstance().getCTSDDVTheoMaCTSDDV(hoaDon.MaCTSDDV.Value)
					: null;

				txbTenKH.Text = "Khách hàng: " + (ct?.TenKH ?? "N/A");
				txbSoPhong.Text = ct?.SoPhong ?? "N/A";
				txbSoNguoi.Text = "N/A";
				txbSoNgayOrGio.Text = "Thời gian thuê: ";
				txbSoNgay.Text = "N/A";

				ls = new List<DichVu_DaChon>();
				if (ct != null)
				{
					var gia = ct.SL > 0 ? ct.ThanhTien / ct.SL : ct.ThanhTien;
					ls.Add(new DichVu_DaChon
					{
						TenDV = ct.TenDV ?? "N/A",
						Gia = gia,
						SoLuong = ct.SL,
						ThanhTien = ct.ThanhTien
					});
				}

				lvDichVuDaSD.ItemsSource = ls;
			}
			catch (Exception ex)
			{
				new DialogCustoms("Lỗi: " + ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
			}
		}

		private void ConfigureButtons(bool showPayment, bool showPrint)
		{
			groupButton.Visibility = showPayment ? Visibility.Visible : Visibility.Hidden;
			btnInHoaDon.Visibility = showPrint ? Visibility.Visible : Visibility.Hidden;
		}

		private HoaDon EnsureHoaDonChoDichVu(bool createIfMissing)
		{
			if (ctSDDV == null) return null;

			var hoaDon = HoaDonBUS.GetInstance().LayHoaDonTheoMaCTSDDV(ctSDDV.MaCTSDDV);
			if (hoaDon != null) return hoaDon;
			if (!createIfMissing) return null;

			var error = string.Empty;
			var hd = new HoaDon
			{
				MaNV = MaNV,
				MaCTSDDV = ctSDDV.MaCTSDDV,
				NgayLap = DateTime.Now,
				TongTien = ctSDDV.ThanhTien
			};

			if (!HoaDonBUS.GetInstance().themHoaDon(hd, out error))
			{
				new DialogCustoms("Thêm hóa đơn thất bại!\nLỗi: " + error, "Thông báo", DialogCustoms.OK).ShowDialog();
				return null;
			}

			hd.MaHD = HoaDonBUS.GetInstance().layMaHDMoiNhat();
			hd.NhanVien = NhanVienBUS.GetInstance().layNhanVienTheoMaNV(MaNV);
			return hd;
		}

		private void LoadThongTinDichVu(CT_SDDichVu ct, bool createHoaDon)
		{
			if (ct == null)
			{
				new DialogCustoms("Không tìm thấy dịch vụ.", "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			var hoaDon = EnsureHoaDonChoDichVu(createHoaDon);
			var nhanVien = hoaDon?.NhanVien ?? NhanVienBUS.GetInstance().layNhanVienTheoMaNV(MaNV);

			var ctPhieuThue = ct.MaCTPT.HasValue
				? CT_PhieuThueBUS.GetInstance().getCTPhieuThueTheoMaCTPT(ct.MaCTPT.Value)
				: null;

			txbTenKH.Text = "Khách hàng: " + (ct.TenKH ?? "N/A");
			txbSoPhong.Text = ctPhieuThue?.SoPhong ?? ct.SoPhong ?? "N/A";
			txbSoNguoi.Text = ctPhieuThue != null ? ctPhieuThue.SoNguoiO.ToString() : "N/A";
			txbSoNgayOrGio.Text = "Thời gian thuê: ";
			txbSoNgay.Text = "N/A";
			txbNhanVien.Text = nhanVien?.HoTen ?? "N/A";
			txbNgayLapHD.Text = (hoaDon?.NgayLap ?? ct.NgayApDung).ToString();
			txbTongTien.Text = string.Format("{0:0,0 VND}", (hoaDon?.TongTien ?? ct.ThanhTien));
			txbSoHoaDon.Text = hoaDon != null ? hoaDon.MaHD.ToString() : "N/A";

			var gia = ct.SL > 0 ? ct.ThanhTien / ct.SL : ct.ThanhTien;
			ls.Add(new DichVu_DaChon
			{
				TenDV = ct.TenDV ?? "N/A",
				Gia = gia,
				SoLuong = ct.SL,
				ThanhTien = ct.ThanhTien
			});

			lvDichVuDaSD.ItemsSource = ls;
		}

		private void HandleThanhToanDichVu(string hinhThucThanhToan)
		{
			if (ctSDDV == null || isDaThanhToanHandled) return;

			var error = string.Empty;
			var trangThai = string.IsNullOrWhiteSpace(ctSDDV.TrangThai) ? "Dịch vụ đã đặt" : ctSDDV.TrangThai;
			if (!CTSDDV_BUS.GetInstance().capNhatTrangThaiSuDung(ctSDDV.MaCTSDDV, trangThai, true, hinhThucThanhToan,
				    out error))
			{
				new DialogCustoms("Thanh toán thất bại!\nLỗi: " + error, "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			isDaThanhToanHandled = true;
			ctSDDV.DaThanhToan = true;
			ctSDDV.HinhThucThanhToan = hinhThucThanhToan;

			var hoaDon = EnsureHoaDonChoDichVu(true);
			if (hoaDon != null)
			{
				txbSoHoaDon.Text = hoaDon.MaHD.ToString();
				txbNgayLapHD.Text = hoaDon.NgayLap.ToString();
				txbTongTien.Text = string.Format("{0:0,0 VND}", hoaDon.TongTien);
				txbNhanVien.Text = hoaDon.NhanVien?.HoTen ?? txbNhanVien.Text;
			}

			ConfigureButtons(false, true);
		}

		private void TryCapNhatTrangThaiSauIn()
		{
			if (!updateTrangThaiOnPrint || ctSDDV == null) return;

			var trangThaiDaSuDung = "Đã sử dụng";
			if (string.Equals(ctSDDV.TrangThai, trangThaiDaSuDung, StringComparison.OrdinalIgnoreCase)) return;

			var error = string.Empty;
			var daThanhToan = ctSDDV.DaThanhToan.HasValue && ctSDDV.DaThanhToan.Value;
			var hinhThuc = string.IsNullOrWhiteSpace(ctSDDV.HinhThucThanhToan) ? "Direct" : ctSDDV.HinhThucThanhToan;

			if (!CTSDDV_BUS.GetInstance().capNhatTrangThaiSuDung(ctSDDV.MaCTSDDV, trangThaiDaSuDung, daThanhToan,
				    hinhThuc, out error))
			{
				new DialogCustoms("Lỗi cập nhật trạng thái!\nLỗi: " + error, "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			ctSDDV.TrangThai = trangThaiDaSuDung;
		}

		private void click_InHoaDon(object sender, RoutedEventArgs e)
		{
			var prevPrintVisibility = btnInHoaDon.Visibility;
			var prevGroupVisibility = groupButton.Visibility;
			try
			{
				btnInHoaDon.Visibility = Visibility.Hidden;
				groupButton.Visibility = Visibility.Hidden;

				var printDialog = new PrintDialog();
				if (printDialog.ShowDialog() == true)
				{
					printDialog.PrintVisual(print, "In hóa đơn");
					new DialogCustoms("In hóa đơn thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
					TryCapNhatTrangThaiSauIn();
				}
			}
			catch (Exception ex)
			{
				new DialogCustoms("In hóa đơn thất bại!\nLỗi: " + ex.Message, "Thông báo", DialogCustoms.OK)
					.ShowDialog();
			}
			finally
			{
				btnInHoaDon.Visibility = prevPrintVisibility;
				groupButton.Visibility = prevGroupVisibility;
			}
		}

		private bool isDaThanhToanHandled = false;

		private void btnDaThanhToan_Click(object sender, RoutedEventArgs e)
		{
			if (ctSDDV != null)
			{
				HandleThanhToanDichVu("Direct");
				return;
			}

			if (isDaThanhToanHandled) return; // Nếu đã xử lý, không làm gì thêm

			var phongInfo = PhongBUS.GetInstance().getDataPhongTheoSoPhong(Phong.MaPhong);
			var loaiPhong = phongInfo != null
				? LoaiPhongBUS.GetInstance().getLoaiPhongTheoMaLoaiPhong(phongInfo.MaLoaiPhong)
				: null;

			var checkOut = new CheckIn_CheckOut(false, Phong, loaiPhong);
			if (checkOut.ShowDialog() != true) return;

			isDaThanhToanHandled = true; // Đánh dấu đã xử lý

			// Sửa trạng thái của phiếu thuê
			var errorSuaCTPT = string.Empty;
			if (!CT_PhieuThueBUS.GetInstance().suaTinhTrangThuePhong(Phong.MaCTPT, "Đã thanh toán", true, "Direct", out errorSuaCTPT))
			{
				new DialogCustoms("Lỗi sửa CTPT\nLỗi: " + errorSuaCTPT, "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			var errorSuaPhong = string.Empty;
			if (!PhongBUS.GetInstance().suaTinhTrangPhong(Phong.MaPhong, "Chưa dọn dẹp", out errorSuaPhong))
			{
				new DialogCustoms("Lỗi sửa trạng thái phòng\nLỗi: " + errorSuaPhong, "Thông báo", DialogCustoms.OK)
					.ShowDialog();
			}

			// Cập nhật thời gian kết thúc thuê phòng
			CT_PhieuThueBUS.GetInstance().capNhatNgayKT(Phong.MaCTPT, ngayKT, out string error);

			// Disable các nút liên quan
			btnDaThanhToan.IsEnabled = false;
			btnDaThanhToan.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#808080"));

			btnQR.IsEnabled = false;
			btnQR.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#808080"));
		}

		private async void btnQR_Click(object sender, RoutedEventArgs e)
		{
			List<ItemData> items = new List<ItemData>();
			foreach (var dv in ls)
			{
				var name = dv.TenDV;
				var quantity = Convert.ToInt32(dv.SoLuong);
				var money = Convert.ToInt32(dv.Gia);
				ItemData item = new ItemData(name, quantity, money);
				items.Add(item);
			}

			if (ctSDDV != null)
			{
				var hoaDon = EnsureHoaDonChoDichVu(false);
				if (hoaDon != null)
				{
					txbSoHoaDon.Text = hoaDon.MaHD.ToString();
					txbNgayLapHD.Text = hoaDon.NgayLap.ToString();
					txbTongTien.Text = string.Format("{0:0,0 VND}", hoaDon.TongTien);
					txbNhanVien.Text = hoaDon.NhanVien?.HoTen ?? txbNhanVien.Text;
				}
			}

			// Nếu PayOS/đối tác không nhận Unicode, bạn có thể đổi lại thành không dấu.
			var noidung = "THANH TOÁN KHÁCH SẠN";

			if (!int.TryParse(txbSoHoaDon.Text, out var soHoaDon))
			{
				soHoaDon = ctSDDV?.MaCTSDDV ?? 0;
				if (soHoaDon <= 0)
				{
					new DialogCustoms("Không tìm thấy số hóa đơn hợp lệ.", "Thông báo", DialogCustoms.OK).ShowDialog();
					return;
				}
			}

			// Lấy thời gian Unix timestamp (mili giây)
			long unixTimeMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			int unixTimestampPart = (int)(unixTimeMilliseconds % 10000); // Lấy 4 chữ số cuối

			// Kết hợp số hóa đơn và phần cuối của Unix timestamp
			var orderID = soHoaDon * 10000 + unixTimestampPart;

			var tongGiaTien = items.Sum(item => item.price * item.quantity);

			PaymentData paymentData = new PaymentData(orderID, tongGiaTien, noidung, items, "https://localhost:3002", "https://localhost:3002");

			try
			{
				CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
				var linkCheckOut = createPayment.checkoutUrl;

				// Mở cửa sổ WebThanhToan và truyền callback xử lý trạng thái
				WebThanhToan webThanhToanWindow = new WebThanhToan(linkCheckOut, HandlePaymentStatus);
				webThanhToanWindow.ShowDialog();
			}
			catch (Exception ex)
			{
				new DialogCustoms($"Có lỗi xảy ra: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
			}
		}

		// Cờ kiểm soát để tránh thông báo bị lặp
		private bool isPaymentHandled = false;

		private void HandlePaymentStatus(string status)
		{
			if (isPaymentHandled) return; // Nếu đã xử lý, không làm gì thêm

			if (status == "CANCELLED")
			{
				// Xử lý khi trạng thái là CANCELLED
				isPaymentHandled = true; // Đặt cờ để tránh lặp
				new DialogCustoms("Thanh toán đã bị hủy!", "Thông báo", DialogCustoms.OK).ShowDialog();
			}
			else if (status == "PAID")
			{
				// Xử lý khi trạng thái là PAID
				isPaymentHandled = true; // Đặt cờ để tránh lặp
				new DialogCustoms("Thanh toán thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
				if (ctSDDV != null)
				{
					HandleThanhToanDichVu("Online");
				}
				else
				{
					btnDaThanhToan_Click(this, new RoutedEventArgs());
				}
			}
		}
	}
}
