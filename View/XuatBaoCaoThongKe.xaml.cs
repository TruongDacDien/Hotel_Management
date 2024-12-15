using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BUS;
using ClosedXML.Excel;
using DAL.DTO;
using Microsoft.Win32;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for XuatBaoCaoThongKe.xaml
	/// </summary>
	public partial class XuatBaoCaoThongKe : Window
	{
		public XuatBaoCaoThongKe()
		{
			InitializeComponent();
		}

		public XuatBaoCaoThongKe(int monthSelected, int yearSelected) : this()
		{
			// Tiếp tục khởi tạo các thành phần khác
			SelectedMonth = monthSelected;
			SelectedYear = yearSelected;

			MonthSelector.ItemsSource = Enumerable.Range(1, 12).Select(m => $"{m}").ToList();
			MonthSelector.SelectedIndex = SelectedMonth - 1;

			YearSelector.Items.Clear();
			for (var year = DateTime.Now.Year - 10; year <= DateTime.Now.Year; year++) // Danh sách 10 năm gần đây
				YearSelector.Items.Add(year);
			YearSelector.SelectedItem = SelectedYear;

			Table_Load();
		}

		public ObservableCollection<HoaDon> BaoCaoData { get; set; }

		public int SelectedMonth { get; set; }

		public int SelectedYear { get; set; }

		public decimal TotalRevenue { get; set; }

		private void Table_Load()
		{
			SelectedMonth = MonthSelector.SelectedIndex + 1;
			SelectedYear = SelectedYear;

			// Giả lập dữ liệu báo cáo (lấy từ BUS)
			BaoCaoData = new ObservableCollection<HoaDon>(HoaDonBUS.GetInstance().GetHoaDons());

			// Lọc dữ liệu theo tháng và năm được chọn
			var filteredData =
				BaoCaoData.Where(hd => hd.NgayLap.Month == SelectedMonth && hd.NgayLap.Year == SelectedYear);

			// Gán dữ liệu đã lọc vào DataGrid
			ReportDataGrid.ItemsSource = filteredData;

			// Tính tổng doanh thu từ dữ liệu đã lọc
			TotalRevenue = filteredData.Sum(hd => hd.TongTien);

			// Hiển thị tổng doanh thu
			TotalRevenueText.Text = $"{TotalRevenue:N0} VND";
		}

		private void OnMonthYearChanged(object sender, SelectionChangedEventArgs e)
		{
			// Xử lý sự kiện khi tháng và năm thay đổi
			Table_Load();
		}

		private void OnExportToExcelClick(object sender, RoutedEventArgs e)
		{
			try
			{
				// Hộp thoại chọn đường dẫn lưu fileclean
				var saveFileDialog = new SaveFileDialog
				{
					Filter = "Excel Files (*.xlsx)|*.xlsx",
					FileName = $"BaoCao_Thang_{MonthSelector.SelectedIndex + 1}_{YearSelector.SelectedItem}.xlsx"
				};

				if (saveFileDialog.ShowDialog() == true)
				{
					var filePath = saveFileDialog.FileName;

					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Báo cáo doanh thu");

						// Ghi một dòng trống ở đầu
						worksheet.Cell(1, 1).Value = "";

						// Ghi tiêu đề "Báo cáo doanh thu"
						var titleCell = worksheet.Cell(2, 1);
						titleCell.Value = "BÁO CÁO DOANH THU";
						titleCell.Style.Font.Bold = true;
						titleCell.Style.Font.FontSize = 16;
						titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

						// Gộp ô cho tiêu đề
						worksheet.Range(2, 1, 2, 5).Merge();

						// Ghi ngày lập báo cáo
						var dateCell = worksheet.Cell(3, 1);
						dateCell.Value = $"Ngày lập: {DateTime.Now:dd/MM/yyyy}";
						dateCell.Style.Font.Bold = true;
						dateCell.Style.Font.FontSize = 14;
						dateCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

						// Gộp ô cho ngày lập báo cáo
						worksheet.Range(3, 1, 3, 5).Merge();

						// Ghi tiêu đề bảng dữ liệu
						var headers = new[]
							{ "Mã hóa đơn", "Ngày lập", "Tên nhân viên", "Mã chi tiết phiếu thuê", "Tổng tiền" };
						for (var i = 0; i < headers.Length; i++)
						{
							var headerCell = worksheet.Cell(5, i + 1);
							headerCell.Value = headers[i];
							headerCell.Style.Font.Bold = true;
							headerCell.Style.Font.FontSize = 12;
							headerCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
							headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
						}

						// Ghi dữ liệu báo cáo
						for (var i = 0; i < BaoCaoData.Count; i++)
						{
							var item = BaoCaoData[i];
							worksheet.Cell(i + 6, 1).Value = item.MaHD;
							worksheet.Cell(i + 6, 2).Value = item.NgayLap.ToString("dd/MM/yyyy hh:mm tt");
							worksheet.Cell(i + 6, 3).Value = item.NhanVien.HoTen;
							worksheet.Cell(i + 6, 4).Value = item.MaCTPT;
							worksheet.Cell(i + 6, 5).Value = item.TongTien;
							worksheet.Cell(i + 6, 5).Style.NumberFormat.Format = "#,##0 ₫";

							// Áp dụng định dạng cho các ô dữ liệu
							for (var j = 1; j <= headers.Length; j++)
							{
								var dataCell = worksheet.Cell(i + 6, j);
								dataCell.Style.Font.FontSize = 12;
								dataCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
								dataCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
							}
						}

						// Dòng sau dữ liệu
						var rowAfterData = BaoCaoData.Count + 7;

						// Ghi "Tổng doanh thu" và giá trị tổng doanh thu
						worksheet.Cell(rowAfterData, 4).Value = "Tổng doanh thu:";
						worksheet.Cell(rowAfterData, 4).Style.Font.Bold = true;
						worksheet.Cell(rowAfterData, 4).Style.Font.FontSize = 12;
						worksheet.Cell(rowAfterData, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

						// Ghi giá trị tổng doanh thu ở cột cuối cùng (cột 5)
						worksheet.Cell(rowAfterData, 5).Value = TotalRevenue;
						worksheet.Cell(rowAfterData, 5).Style.NumberFormat.Format = "#,##0 ₫";
						worksheet.Cell(rowAfterData, 5).Style.Font.Bold = true;
						worksheet.Cell(rowAfterData, 5).Style.Font.FontSize = 12;
						worksheet.Cell(rowAfterData, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

						// Tự động điều chỉnh kích thước cột
						worksheet.Column(1).AdjustToContents();
						worksheet.Column(2).AdjustToContents();
						worksheet.Column(3).AdjustToContents();
						worksheet.Column(4).AdjustToContents();
						worksheet.Column(5).Width = Math.Max(worksheet.Column(5).Width, 20);

						// Lưu file Excel
						workbook.SaveAs(filePath);

						// Hiển thị thông báo thành công
						new DialogCustoms("Xuất dữ liệu thành công!", "Thông báo", DialogCustoms.OK).Show();
					}

					// Mở file Excel sau khi lưu thành công
					Process.Start("explorer.exe", filePath);
				}
			}
			catch (Exception ex)
			{
				new DialogCustoms($"Có lỗi xảy ra khi xuất file Excel: {ex.Message}", "Lỗi", DialogCustoms.OK).Show();
			}
		}
	}
}