using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BUS;
using DAL.Data;
using DAL.DTO;

namespace GUI.UserControls
{
    /// <summary>
    /// Interaction logic for uc_ThongKe.xaml
    /// </summary>
    public partial class uc_ThongKe : UserControl
    {
        public SeriesCollection PieSeriesCollection { get; set; }
        public SeriesCollection CartesianSeriesCollection { get; set; }
        public Axis XAxis { get; set; }
        public Axis YAxis { get; set; }
        public string[] Labels { get; set; }
        private bool _isInitialized = false;
        public uc_ThongKe()
        {
            InitializeComponent();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            MonthSelector.ItemsSource = Enumerable.Range(1, 12).Select(m => $"{m}").ToList();
            MonthSelector.SelectedIndex = currentMonth - 1;

            YearSelector.Items.Clear();
            for (int year = DateTime.Now.Year - 10; year <= DateTime.Now.Year; year++) // Danh sách 10 năm gần đây
            {
                YearSelector.Items.Add(year);
            }

            YearSelector.SelectedItem = currentYear;


            init_CartesianChart();

            init_PieChart();
            _isInitialized = true;

            UpdatePieChart(currentMonth, currentYear);
            UpdateCartesianChart(currentYear);

            DataContext = this;
        }

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
                    Values = new ChartValues<double> {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
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
                MinValue = -500000,  // Giá trị nhỏ nhất
                MaxValue = 2000000,  // Giá trị lớn nhất
                Separator = new LiveCharts.Wpf.Separator  // Thêm namespace đầy đủ ở đây
                {
                    Step = 250000     // Khoảng cách giữa các mốc
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
            int selectedMonth = MonthSelector.SelectedIndex + 1;
            Console.WriteLine(selectedMonth);
            int selectedYear = int.Parse(YearSelector.SelectedItem.ToString());
            Console.WriteLine(selectedYear);
            UpdateCartesianChart(selectedYear);
            UpdatePieChart(selectedMonth, selectedYear);
        }
        private void UpdateCartesianChart(int year)
        {
            List<CTHD> kq = ThongKeBUS.GetInstance().LoadThongKeTheoNam(year);

            var revenueRoomValues = new ChartValues<decimal>();
            var revenueServiceValues = new ChartValues<decimal>();
            var totalRevenueValues = new ChartValues<decimal>();

            for (int month = 1; month <= 12; month++)
            {
                // Lọc dữ liệu cho tháng hiện tại
                var filteredData = kq.Where(item => item.NgayLap.Month == month);

                // Tính tổng giá trị cho tháng hiện tại
                decimal monthlyRevenueRoom = filteredData.Sum(item => item.TienPhong);
                decimal monthlyRevenueService = filteredData.Sum(item => item.TienDV);
                decimal monthlyTotalRevenue = filteredData.Sum(item => item.TongTien);

                // Thêm vào giá trị biểu đồ
                revenueRoomValues.Add(monthlyRevenueRoom);
                revenueServiceValues.Add(monthlyRevenueService);
                totalRevenueValues.Add(monthlyTotalRevenue);
            }

            CartesianSeriesCollection[0].Values = revenueRoomValues;    // Doanh thu phòng
            CartesianSeriesCollection[1].Values = revenueServiceValues; // Doanh thu dịch vụ
            CartesianSeriesCollection[2].Values = totalRevenueValues;
        }

        // Hàm để kiểm tra dữ liệu nào thuộc về tháng và năm đã chọn
        private bool IsDataForMonthYear(int index, int month, int year)
        {
            // Lấy ngày từ trục X của trục X và chuyển thành tháng/năm
            var dateParts = XAxis.Labels[index].Split('-');  // Giả sử XAxis.Labels có định dạng "mm-yyyy"
            int dataMonth = int.Parse(dateParts[0]);  // Tháng
            int dataYear = int.Parse(dateParts[1]);   // Năm

            return dataMonth == month && dataYear == year;
        }
        public void UpdatePieChart(int month, int year)
        {
            ThongKe thongKe = ThongKeBUS.GetInstance().LoadThongKeTheoThangNam(month, year);
            // Cập nhật các series của biểu đồ
            if (thongKe != null)
            {
                // Gán dữ liệu doanh thu vào series
                PieSeriesCollection[0].Values = new ChartValues<decimal> { thongKe.DoanhthuPhong };
                PieSeriesCollection[1].Values = new ChartValues<decimal> { thongKe.DoanhthuDichVu };
                txbRevenuePhongThisMonth.Text = $"{thongKe.DoanhthuPhong.ToString("N0")} VNĐ";
                txbRevenueDVThisMonth.Text = $"{thongKe.DoanhthuDichVu.ToString("N0")} VNĐ";
                txtsoPhong.Text = thongKe.Sophong.ToString() + " Phòng";
            }
            else
            {
                txbRevenuePhongThisMonth.Text = "0 VNĐ";
                txbRevenueDVThisMonth.Text = "0 VNĐ";
                txtsoPhong.Text = "0 Phòng";

                PieSeriesCollection[0].Values = new ChartValues<decimal> { 0 };
                PieSeriesCollection[1].Values = new ChartValues<decimal> { 0 };
               
            }

            foreach (var series in PieSeriesCollection)
            {
                if (series is PieSeries pieSeries)
                {
                    pieSeries.LabelPoint = chartPoint => $"{chartPoint.Y.ToString("N0")} VNĐ";
                }
            }
        }

    }
}
