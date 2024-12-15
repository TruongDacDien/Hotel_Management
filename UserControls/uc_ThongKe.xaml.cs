using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BUS;
using GUI.View;
using LiveCharts;
using LiveCharts.Wpf;
using Separator = LiveCharts.Wpf.Separator;

namespace GUI.UserControls
{
	/// <summary>
	///     Interaction logic for uc_ThongKe.xaml
	/// </summary>
	public partial class uc_ThongKe : UserControl
	{
		private readonly bool _isInitialized;

		public uc_ThongKe()
		{
			InitializeComponent();

			var currentMonth = DateTime.Now.Month;
			var currentYear = DateTime.Now.Year;

			MonthSelector.ItemsSource = Enumerable.Range(1, 12).Select(m => $"{m}").ToList();
			MonthSelector.SelectedIndex = currentMonth - 1;

			YearSelector.Items.Clear();
			for (var year = DateTime.Now.Year - 10; year <= DateTime.Now.Year; year++) // Danh sách 10 năm gần đây
				YearSelector.Items.Add(year);

			YearSelector.SelectedItem = currentYear;


			init_CartesianChart();

			init_PieChart();
			_isInitialized = true;

			UpdatePieChart(currentMonth, currentYear);
			UpdateCartesianChart(currentYear);

			DataContext = this;
		}

		public SeriesCollection PieSeriesCollection { get; set; }
		public SeriesCollection CartesianSeriesCollection { get; set; }
		public Axis XAxis { get; set; }
		public Axis YAxis { get; set; }
		public string[] Labels { get; set; }

		public void init_CartesianChart()
		{
			CartesianSeriesCollection = new SeriesCollection
			{
				new LineSeries
				{
					Title = "Doanh thu phòng",
					Values = new ChartValues<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
				},
				new LineSeries
				{
					Title = "Doanh thu DV",
					Values = new ChartValues<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					PointGeometry = null
				},
				new LineSeries
				{
					Title = "Tổng doanh thu",
					Values = new ChartValues<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					PointGeometry = DefaultGeometries.Square,
					PointGeometrySize = 15
				}
			};

			XAxis = new Axis
			{
				Title = "Tháng",
				Labels = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" }
			};

			// Cấu hình trục dọc (Y-Axis)
			YAxis = new Axis
			{
				Title = "Doanh thu (VND)",
				MinValue = -500000, // Giá trị nhỏ nhất
				MaxValue = 2000000, // Giá trị lớn nhất
				Separator = new Separator // Thêm namespace đầy đủ ở đây
				{
					Step = 250000 // Khoảng cách giữa các mốc
				}
			};
		}

		public void init_PieChart()
		{
			PieSeriesCollection = new SeriesCollection
			{
				new PieSeries
				{
					Title = "Doanh thu phòng",
					Values = new ChartValues<decimal> { 0 },
					DataLabels = true,
					LabelPoint = chartPoint => $"{chartPoint.Y.ToString("N0")} VNĐ"
				},
				new PieSeries
				{
					Title = "Doanh thu dịch vụ",
					Values = new ChartValues<decimal> { 0 },
					DataLabels = true,
					LabelPoint = chartPoint => $"{chartPoint.Y.ToString("N0")} VNĐ"
				}
			};

			//UpdatePieChart(12, 2024);
		}

		private void OnMonthYearChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!_isInitialized) return;
			var selectedMonth = MonthSelector.SelectedIndex + 1;
			Console.WriteLine(selectedMonth);
			var selectedYear = int.Parse(YearSelector.SelectedItem.ToString());
			Console.WriteLine(selectedYear);
			UpdateCartesianChart(selectedYear);
			UpdatePieChart(selectedMonth, selectedYear);
		}

		private void UpdateCartesianChart(int year)
		{
			var kq = ThongKeBUS.GetInstance().LoadThongKeTheoNam(year);

			var revenueRoomValues = new ChartValues<decimal>();
			var revenueServiceValues = new ChartValues<decimal>();
			var totalRevenueValues = new ChartValues<decimal>();

			for (var month = 1; month <= 12; month++)
			{
				// Lọc dữ liệu cho tháng hiện tại
				var filteredData = kq.Where(item => item.NgayLap.Month == month);

				// Tính tổng giá trị cho tháng hiện tại
				var monthlyRevenueRoom = filteredData.Sum(item => item.TienPhong);
				var monthlyRevenueService = filteredData.Sum(item => item.TienDV);
				var monthlyTotalRevenue = filteredData.Sum(item => item.TongTien);

				// Thêm vào giá trị biểu đồ
				revenueRoomValues.Add(monthlyRevenueRoom);
				revenueServiceValues.Add(monthlyRevenueService);
				totalRevenueValues.Add(monthlyTotalRevenue);
			}

			CartesianSeriesCollection[0].Values = revenueRoomValues; // Doanh thu phòng
			CartesianSeriesCollection[1].Values = revenueServiceValues; // Doanh thu dịch vụ
			CartesianSeriesCollection[2].Values = totalRevenueValues;
		}

		// Hàm để kiểm tra dữ liệu nào thuộc về tháng và năm đã chọn
		private bool IsDataForMonthYear(int index, int month, int year)
		{
			// Lấy ngày từ trục X của trục X và chuyển thành tháng/năm
			var dateParts = XAxis.Labels[index].Split('-'); // Giả sử XAxis.Labels có định dạng "mm-yyyy"
			var dataMonth = int.Parse(dateParts[0]); // Tháng
			var dataYear = int.Parse(dateParts[1]); // Năm

			return dataMonth == month && dataYear == year;
		}

		public void UpdatePieChart(int month, int year)
		{
			var thongKe = ThongKeBUS.GetInstance().LoadThongKeTheoThangNam(month, year);
			decimal totalRevenue = 0;
			// Cập nhật các series của biểu đồ
			if (thongKe != null)
			{
				// Gán dữ liệu doanh thu vào series
				totalRevenue = thongKe.DoanhthuPhong + thongKe.DoanhthuDichVu;
				PieSeriesCollection[0].Values = new ChartValues<decimal> { thongKe.DoanhthuPhong };
				PieSeriesCollection[1].Values = new ChartValues<decimal> { thongKe.DoanhthuDichVu };
				txbRevenuePhongThisMonth.Text = $"{thongKe.DoanhthuPhong.ToString("N0")} VNĐ";
				txbRevenueDVThisMonth.Text = $"{thongKe.DoanhthuDichVu.ToString("N0")} VNĐ";
				txtsoPhong.Text = thongKe.Sophong + " Phòng";

				foreach (var series in PieSeriesCollection)
					if (series is PieSeries pieSeries)
					{
						pieSeries.LabelPoint = chartPoint =>
						{
							var percentage = (decimal)chartPoint.Y / totalRevenue * 100;
							return $"{percentage.ToString("N2")}%";
						};
						pieSeries.Fill = new SolidColorBrush(GetColorForSeries(pieSeries));
					}
			}
			else
			{
				txbRevenuePhongThisMonth.Text = "0 VNĐ";
				txbRevenueDVThisMonth.Text = "0 VNĐ";
				txtsoPhong.Text = "0 Phòng";

				PieSeriesCollection[0].Values = new ChartValues<decimal> { 0 };
				PieSeriesCollection[1].Values = new ChartValues<decimal> { 0 };
			}
		}

		private Color GetColorForSeries(PieSeries series)
		{
			switch (PieSeriesCollection.IndexOf(series))
			{
				case 0: return (Color)ColorConverter.ConvertFromString("#FFFEC007");
				case 1: return (Color)ColorConverter.ConvertFromString("#FF2195F2");
				default: return Colors.Gray;
			}
		}

		private void btnBaoCao_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (MonthSelector == null || YearSelector == null)
					throw new NullReferenceException("MonthSelector hoặc YearSelector bị null.");

				var selectedMonth = MonthSelector.SelectedIndex + 1;
				var selectedYear = int.Parse(YearSelector.SelectedItem.ToString());

				Console.WriteLine($"Selected Month: {selectedMonth}, Selected Year: {selectedYear}");

				var thongKe = new XuatBaoCaoThongKe(selectedMonth, selectedYear);
				thongKe.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}