﻿using DTO;
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

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_room.Visibility = Visibility.Collapsed;
                tt_room_selected.Visibility = Visibility.Collapsed;
                tt_bill.Visibility = Visibility.Collapsed;
                tt_person.Visibility = Visibility.Collapsed;
                tt_room_management.Visibility = Visibility.Collapsed;
                tt_roomtype.Visibility = Visibility.Collapsed;
                tt_service.Visibility = Visibility.Collapsed;
                tt_servicetype.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_room.Visibility = Visibility.Visible;
                tt_room_selected.Visibility = Visibility.Visible;
                tt_bill.Visibility = Visibility.Visible;
                tt_person.Visibility = Visibility.Visible;
                tt_room_management.Visibility = Visibility.Visible;
                tt_roomtype.Visibility = Visibility.Visible;
                tt_service.Visibility = Visibility.Visible;
                tt_servicetype.Visibility = Visibility.Visible;
            }
        }

        private void Tg_btn_Unchecked(object sender, RoutedEventArgs e)
        {
            //img_bg
        }
    }
}
