using DAL.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class ThongKe_DAL
    {
        private static ThongKe_DAL Instance;

        private ThongKe_DAL() { }

        public static ThongKe_DAL GetInstance()
        {
            if (Instance == null)
            {
                Instance = new ThongKe_DAL();
            }
            return Instance;
        }

        public ThongKe LoadThongKeTheoThangNam(int month, int year)
        {
            ThongKe kq = new ThongKe();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT SUM(HD.TongTien) AS TongTien,
                                    SUM(CTPT.TienPhong) AS TienPhong,
                                    SUM(CTDV.ThanhTien) AS TienDV,
                                    COUNT(DISTINCT CTPT.SoPhong) AS SoPhong,
                                    GROUP_CONCAT(DISTINCT CTPT.SoPhong) AS DanhSachPhong
                                    FROM HoaDon HD
                                    LEFT JOIN CT_PhieuThue CTPT on CTPT.MaCTPT = HD.MaCTPT
                                    LEFT JOIN CT_SDDichVu CTDV on CTDV.MaCTPT = CTPT.MaCTPT
                                    WHERE MONTH(NgayLap) = @month AND YEAR(NgayLap) = @year;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kq.DoanhthuDichVu = reader.GetDecimal(reader.GetOrdinal("TienDV"));
                            kq.DoanhthuPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong"));
                            kq.TongDoanhThu = reader.GetDecimal(reader.GetOrdinal("TongTien"));
                            kq.Sophong = reader.GetInt32(reader.GetOrdinal("SoPhong"));
                            string danhSachPhong = reader["DanhSachPhong"].ToString();
                            Console.WriteLine($"Danh sách các phòng đã đếm: {danhSachPhong}");
                        }
                    }
                }
                Console.WriteLine(kq.TongDoanhThu.ToString()+" "+kq.DoanhthuDichVu.ToString()+" "+kq.DoanhthuPhong.ToString());
                return kq;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<CTHD> LoadThongKeTheoNam(int year)
        {
            List<CTHD> kq = new List<CTHD>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT HD.TongTien,
                                    CTPT.TienPhong,
                                    CTDV.ThanhTien,
                                    HD.NgayLap
                                    FROM HoaDon HD
                                    LEFT JOIN CT_PhieuThue CTPT on CTPT.MaCTPT = HD.MaCTPT
                                    LEFT JOIN CT_SDDichVu CTDV on CTDV.MaCTPT = CTPT.MaCTPT
                                    WHERE YEAR(NgayLap) = @year";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@year", year);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            kq.Add(new CTHD
                            {
                                TienPhong = reader.IsDBNull(reader.GetOrdinal("TienPhong")) ? 0 : reader.GetDecimal("TienPhong"),
                                TienDV = reader.IsDBNull(reader.GetOrdinal("ThanhTien")) ? 0 : reader.GetDecimal("ThanhTien"),
                                TongTien = reader.IsDBNull(reader.GetOrdinal("TongTien")) ? 0 : reader.GetDecimal("TongTien"),
                                NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),

                            });
                            Console.WriteLine(kq);
                        }
                    }
                }
                Console.WriteLine();
                return kq;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
