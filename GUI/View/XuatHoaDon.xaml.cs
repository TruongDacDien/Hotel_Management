﻿using System;
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

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for XuatHoaDon.xaml
	/// </summary>
	public partial class XuatHoaDon : Window
	{
		private readonly List<DichVu_DaChon> ls;
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

				// Tính tiền phòng và tiền dịch vụ
				var tienPhong = PhongBUS.GetInstance().tinhTienPhong(Phong, cT_PhieuThue, ngayKT);
				var tienDV = CTSDDV_BUS.GetInstance().tinhTongTienDichVuTheoMaCTPT(Phong.MaCTPT);

				// Hiển thị thông tin chi tiết
				txbSoPhong.Text = Phong.MaPhong;

				// Phân tích số ngày và giờ từ tổng thời gian thuê
				var time = ngayKT - cT_PhieuThue.NgayBD;
				var songay = (int)time.TotalDays;
				var sogio = time.Hours + (time.Seconds > 0 ? 1 : 0); // Làm tròn giờ nếu có phút dư

				var thoiGianThue = $"{songay} ngày {sogio} giờ";

				// Hiển thị thời gian thuê
				txbSoNgayOrGio.Text = "Thời gian thuê: ";
				txbSoNgay.Text = thoiGianThue;

				txbTenKH.Text = "Khách hàng: " + Phong.TenKH;
				txbSoNguoi.Text = Phong.SoNguoi.ToString();
				txbNhanVien.Text = NhanVienBUS.GetInstance().layNhanVienTheoMaNV(MaNV).HoTen;
				txbNgayLapHD.Text = ngayKT.ToString();
				txbTongTien.Text = string.Format("{0:0,0 VND}", (tienDV ?? 0) + tienPhong);

				var hoaDonTemp = HoaDonBUS.GetInstance().LayHoaDonThepMaCTPT(Phong.MaCTPT);
				if ( hoaDonTemp == null)
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
						new DialogCustoms("Thêm hóa đơn thất bại!\nLỗi: " + error, "Thông báo", DialogCustoms.OK)
							.ShowDialog();

					// Lấy mã hóa đơn mới nhất
					txbSoHoaDon.Text = HoaDonBUS.GetInstance().layMaHDMoiNhat().ToString();

					// Cập nhật tiền phòng cho phiếu thuê
					var errorCapNhatCTPT = string.Empty;
					if (!CT_PhieuThueBUS.GetInstance().capNhatTien(ph.MaCTPT, tienPhong, out errorCapNhatCTPT))
						new DialogCustoms("Lỗi cập nhật CTPT\nLỗi: " + errorCapNhatCTPT, "Thông báo", DialogCustoms.OK)
							.ShowDialog();
				}
				else
				{
					txbSoHoaDon.Text = hoaDonTemp.MaHD.ToString();
					hoaDonTemp.MaNV = MaNV;
					hoaDonTemp.NgayLap = ngayKT;
					hoaDonTemp.TongTien = decimal.Parse(((tienDV ?? 0) + tienPhong).ToString());

					if (!HoaDonBUS.GetInstance().capNhatHoaDon(hoaDonTemp))new DialogCustoms("Cập nhật hóa đơn thất bại!\nLỗi: " + error, "Thông báo", DialogCustoms.OK).ShowDialog();
					// Cập nhật tiền phòng cho phiếu thuê
					var errorCapNhatCTPT = string.Empty;
					if (!CT_PhieuThueBUS.GetInstance().capNhatTien(hoaDonTemp.MaCTPT, tienPhong, out errorCapNhatCTPT))
						new DialogCustoms("Lỗi cập nhật CTPT\nLỗi: " + errorCapNhatCTPT, "Thông báo", DialogCustoms.OK)
							.ShowDialog();
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
			}
			catch (Exception ex)
			{
				new DialogCustoms("Lỗi load thông tin!\nLỗi: " + ex.Message, "Thông báo", DialogCustoms.OK)
					.ShowDialog();
			}
		}


		public XuatHoaDon(HoaDon hoaDon) : this()
		{
			try
			{
				groupButton.Visibility = Visibility.Hidden;
				txbNhanVien.Text = hoaDon.NhanVien.HoTen;
				txbSoPhong.Text = hoaDon.CT_PhieuThue.SoPhong;

				var ngayBD = hoaDon.CT_PhieuThue.NgayBD;
				var ngayKT = hoaDon.CT_PhieuThue.NgayKT;

				// Phân tích số ngày và giờ
				var time = ngayKT - ngayBD;
				var songay = (int)time.TotalDays;
				var sogio = time.Hours + (time.Seconds > 0 ? 1 : 0);

				var thoiGianThue = $"{songay} ngày {sogio} giờ";

				// Hiển thị thời gian thuê
				txbSoNgayOrGio.Text = "Thời gian thuê: ";
				txbSoNgay.Text = thoiGianThue;

				txbSoHoaDon.Text = hoaDon.MaHD.ToString();
				txbTenKH.Text = "Khách hàng: " +
								KhachHangBUS.GetInstance().layTenKhachHangTheoMaPT(hoaDon.CT_PhieuThue.MaPhieuThue);
				txbSoNguoi.Text = hoaDon.CT_PhieuThue.SoNguoiO.ToString();
				txbNgayLapHD.Text = hoaDon.NgayLap.ToString();
				txbTongTien.Text = string.Format("{0:0,0 VND}", hoaDon.TongTien);

				// Danh sách dịch vụ đã sử dụng
				ls = new List<DichVu_DaChon>(CTSDDV_BUS.GetInstance().getCTSDDVtheoMaCTPT(hoaDon.MaCTPT));
				// Thêm dịch vụ thuê phòng (theo ngày) vào danh sách dịch vụ đã sử dụng
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

				// Thêm dịch vụ thuê phòng (theo giờ) vào danh sách dịch vụ đã sử dụng
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

				// Thêm danh sách dịch vụ khác
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
				groupButton.Visibility = Visibility.Hidden;
				var printDialog = new PrintDialog();
				if (printDialog.ShowDialog() == true)
				{
					printDialog.PrintVisual(print, "In Hóa đơn");
					new DialogCustoms("In hóa đơn thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
				}
			}
			catch (Exception ex)
			{
				new DialogCustoms("In hóa đơn thất bại! \n Lỗi: " + ex.Message, "Thông báo", DialogCustoms.OK)
					.ShowDialog();
			}
			finally
			{
				btnInHoaDon.Visibility = Visibility.Visible;
				groupButton.Visibility = Visibility.Visible;
			}
		}

		private bool isDaThanhToanHandled = false;

		private void btnDaThanhToan_Click(object sender, RoutedEventArgs e)
		{
			if (isDaThanhToanHandled) return; // Nếu đã xử lý, không làm gì thêm
			isDaThanhToanHandled = true; // Đặt cờ để đánh dấu đã xử lý

			// Sửa trạng thái của phiếu thuê
			var errorSuaCTPT = string.Empty;
			if (!CT_PhieuThueBUS.GetInstance().suaTinhTrangThuePhong(Phong.MaCTPT, "Đã thanh toán", out errorSuaCTPT))
			{
				new DialogCustoms("Lỗi sửa CTPT\nLỗi: " + errorSuaCTPT, "Thông báo", DialogCustoms.OK).ShowDialog();
				return;
			}

			// Cập nhật thời gian kết thúc thuê phòng
			CT_PhieuThueBUS.GetInstance().capNhatNgayKT(Phong.MaCTPT, ngayKT, out string error);

			// Disable các nút liên quan
			btnDaThanhToan.IsEnabled = false;
			btnDaThanhToan.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#808080"));
			btnQR.IsEnabled = false;
			btnQR.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#808080"));

			// Tắt ngrok
			StopNgrok();
		}

		private async void btnQR_Click(object sender, RoutedEventArgs e)
		{
			// Chạy ngrok và chờ đến khi hoàn thành
			var urls = await RunNgrok();

			if (urls.CancelUrl == null || urls.ReturnUrl == null)
			{
				new DialogCustoms("Không thể lấy URL từ Ngrok.", "Lỗi", DialogCustoms.OK).ShowDialog();
				return;
			}

			List<ItemData> items = new List<ItemData>();
			foreach (var dv in ls)
			{
				var name = dv.TenDV;
				var quantity = Convert.ToInt32(dv.SoLuong);
				var money = Convert.ToInt32(dv.Gia);
				ItemData item = new ItemData(name, quantity, money);
				items.Add(item);
			}

			var noidung = "THANH TOAN KHACH SAN";
			var orderID = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); // Tính bằng mili giây
			var tongGiaTien = items.Sum(item => item.price * item.quantity);
			var cancelUrl = urls.CancelUrl;
			var returnUrl = urls.ReturnUrl;

			PaymentData paymentData = new PaymentData(orderID, tongGiaTien, noidung, items, cancelUrl, returnUrl);

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
				btnDaThanhToan_Click(this, new RoutedEventArgs());
			}
		}

		private async Task<(string CancelUrl, string ReturnUrl)> RunNgrok()
		{
			try
			{
				var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				var scriptPath = System.IO.Path.Combine(baseDirectory, "update-ngrok.ps1");
				var ngrokPath = System.IO.Path.Combine(baseDirectory, "ngrok.exe");

				if (!System.IO.File.Exists(scriptPath) || !System.IO.File.Exists(ngrokPath))
				{
					new DialogCustoms("Không tìm thấy file update-ngrok.ps1 hoặc ngrok.exe trong thư mục ứng dụng.", "Lỗi", DialogCustoms.OK).ShowDialog();
					return (null, null);
				}

				StopNgrok();

				var processStartInfo = new ProcessStartInfo
				{
					FileName = "powershell.exe",
					Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				using (var process = new Process { StartInfo = processStartInfo })
				{
					process.Start();
					string output = await process.StandardOutput.ReadToEndAsync();
					string error = await process.StandardError.ReadToEndAsync();
					Console.WriteLine("Output:\n" + output);
					Console.WriteLine("Error:\n" + error);

					var cancelUrl = ParseSetting(output, "cancelUrl");
					var returnUrl = ParseSetting(output, "returnUrl");

					if (!string.IsNullOrEmpty(cancelUrl) && !string.IsNullOrEmpty(returnUrl))
					{
						SettingsHelper.UpdateSettings(cancelUrl, returnUrl);
						return (cancelUrl, returnUrl);
					}
					else
					{
						new DialogCustoms("Failed to parse Ngrok URLs from script output.", "Lỗi", DialogCustoms.OK).ShowDialog();
						return (null, null);
					}
				}
			}
			catch (Exception ex)
			{
				new DialogCustoms($"Có lỗi xảy ra khi chạy script: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
				return (null, null);
			}
		}

		private string ParseSetting(string output, string key)
		{
			Console.WriteLine("Output from script:\n" + output);

			// Split by lines and search for the key
			var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				Console.WriteLine($"Checking line: {line}");
				if (line.StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
				{
					var value = line.Substring(key.Length + 1).Trim();
					Console.WriteLine($"Found {key}: {value}");
					return value;
				}
			}

			Console.WriteLine($"Key '{key}' not found in output.");
			return null;
		}

		private void StopNgrok()
		{
			try
			{
				// Tìm và kết thúc process của ngrok
				foreach (var process in Process.GetProcessesByName("ngrok"))
				{
					process.Kill();
					process.WaitForExit(); // Chờ process dừng hoàn toàn
				}
			}
			catch (Exception ex)
			{
				new DialogCustoms($"Có lỗi xảy ra khi tắt ngrok: {ex.Message}", "Lỗi", DialogCustoms.OK).ShowDialog();
			}
		}
	}
}