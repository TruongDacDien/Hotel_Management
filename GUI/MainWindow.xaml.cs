using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string connectionString;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Truy xuất chuỗi kết nối từ App.config
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            // Tạo danh sách để chứa dữ liệu nhân viên
            List<NhanVien> nhanVienList = new List<NhanVien>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();  // Mở kết nối

                    string query = "SELECT * FROM NHANVIEN";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NhanVien nhanVien = new NhanVien
                            {
                                MANV = reader.GetInt32("MANV"),
                                TENNV = reader.GetString("TENNV"),
                                CHUCVU = reader.GetString("CHUCVU"),
                                SDT = reader.GetString("SDT"),
                                DIACHI = reader.GetString("DIACHI")
                            };

                            nhanVienList.Add(nhanVien);
                        }
                    }

                    // Hiển thị dữ liệu lên DataGrid
                    NhanVienDataGrid.ItemsSource = nhanVienList;

                    MessageBox.Show("Dữ liệu đã được tải thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // Đảm bảo đóng kết nối sau khi hoàn thành
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
