using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using DAL.DTO;

namespace DAL.Data
{
    public class PhongDAL
    {
        private static PhongDAL Instance;

        private PhongDAL() { }

        public static PhongDAL GetInstance()
        {
            if (Instance == null)
            {
                Instance = new PhongDAL();
            }
            return Instance;
        }

        // Lấy dữ liệu phòng theo ngày
        public List<Phong_Custom> getDataPhongTheoNgay(DateTime? ngayChon)
        {
            List<Phong_Custom> ls = new List<Phong_Custom>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = @"
                        SELECT p.SoPhong, p.TinhTrang, p.LoaiPhong.TenLoaiPhong, 
                               ct.MaCTPT, ct.NgayBD, ct.NgayKT, ct.TinhTrangThue, 
                               ct.PhieuThue.KhachHang.TenKH, ct.SoNguoiO
                        FROM Phong p
                        LEFT JOIN CT_PhieuThue ct ON p.SoPhong = ct.SoPhong
                        WHERE ct.NgayBD <= @NgayChon AND ct.NgayKT >= @NgayChon
                            AND ct.TinhTrangThue != 'Đã thanh toán' AND p.IsDeleted = 0";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NgayChon", ngayChon);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ls.Add(new Phong_Custom
                            {
                                MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
                                TenKH = reader.IsDBNull(reader.GetOrdinal("TenKH")) ? "" : reader.GetString(reader.GetOrdinal("TenKH")),
                                MaPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
                                DonDep = reader.GetString(reader.GetOrdinal("TinhTrang")),
                                TinhTrang = reader.IsDBNull(reader.GetOrdinal("TinhTrangThue")) ? "Phòng trống" : reader.GetString(reader.GetOrdinal("TinhTrangThue")),
                                LoaiPhong = reader.GetString(reader.GetOrdinal("TenLoaiPhong")),
                                NgayDen = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
                                SoNgayO = reader.IsDBNull(reader.GetOrdinal("NgayBD")) ? 0 : (int)(reader.GetDateTime(reader.GetOrdinal("NgayBD")).Subtract(reader.GetDateTime(reader.GetOrdinal("NgayKT"))).TotalDays) + 1,
                                SoGio = reader.IsDBNull(reader.GetOrdinal("NgayBD")) ? 0 : (int)(reader.GetDateTime(reader.GetOrdinal("NgayBD")).Subtract(reader.GetDateTime(reader.GetOrdinal("NgayKT"))).TotalHours),
                                NgayDi = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
                                SoNguoi = reader.IsDBNull(reader.GetOrdinal("SoNguoiO")) ? 0 : reader.GetInt32(reader.GetOrdinal("SoNguoiO"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log lỗi nếu cần
            }

            return ls;
        }

        // Cập nhật tình trạng phòng
        public bool suaTinhTrangPhong(string maPhong, string text, out string error)
        {
            error = string.Empty;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE Phong SET TinhTrang = @TinhTrang WHERE SoPhong = @MaPhong";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TinhTrang", text);
                    cmd.Parameters.AddWithValue("@MaPhong", maPhong);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        error = "Không tồn tại phòng có số phòng: " + maPhong;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Lấy giá tiền của phòng theo mã phòng
        public decimal layGiaTienTheoMaPhong(string maphong, bool isday)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT LoaiPhong.GiaNgay, LoaiPhong.GiaGio FROM Phong p " +
                               "INNER JOIN LoaiPhong ON p.MaLoaiPhong = LoaiPhong.MaLoaiPhong " +
                               "WHERE p.SoPhong = @MaPhong";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhong", maphong);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (isday)
                            return reader.GetDecimal(reader.GetOrdinal("GiaNgay"));
                        else
                            return reader.GetDecimal(reader.GetOrdinal("GiaGio"));
                    }
                }
            }

            return 0; // Default giá tiền
        }

        // Lấy danh sách phòng trống
        public List<PhongTrong> getPhongTrong(DateTime? ngayBD, DateTime? ngayKT)
        {
            List<PhongTrong> lsPTrong = new List<PhongTrong>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = @"
                        SELECT p.SoPhong, p.LoaiPhong.TenLoaiPhong
                        FROM Phong p
                        WHERE p.IsDeleted = 0 AND p.SoPhong NOT IN (
                            SELECT ct.SoPhong
                            FROM CT_PhieuThue ct
                            WHERE (ct.NgayBD <= @NgayBD AND ct.NgayKT >= @NgayKT) 
                                OR (ct.NgayBD >= @NgayBD AND ct.NgayBD <= @NgayKT) 
                                OR (ct.NgayKT >= @NgayBD AND ct.NgayKT <= @NgayKT) 
                                OR (ct.NgayBD >= @NgayBD AND ct.NgayKT <= @NgayKT)
                                AND ct.TinhTrangThue != 'Đã thanh toán'
                        )";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NgayBD", ngayBD);
                    cmd.Parameters.AddWithValue("@NgayKT", ngayKT);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lsPTrong.Add(new PhongTrong
                            {
                                SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
                                TenLoaiPhong = reader.GetString(reader.GetOrdinal("TenLoaiPhong"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log lỗi nếu cần
            }

            return lsPTrong;
        }

        // Thêm mới phòng
        public bool addDataPhong(Phong phong)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {

                    string query = "INSERT INTO Phong (SoPhong, MaLoaiPhong, DonDep, IsDeleted) VALUES (@SoPhong, @MaLoaiPhong, @TinhTrang, 0)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
                    cmd.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);
                    cmd.Parameters.AddWithValue("@TinhTrang", phong.TinhTrang);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log error if needed
                return false;
            }
        }

        // Cập nhật phòng
        public bool capNhatPhong(Phong phong)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    
                    string query = "UPDATE Phong SET MaLoaiPhong = @MaLoaiPhong, DonDep = @TinhTrang WHERE SoPhong = @SoPhong";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);
                    cmd.Parameters.AddWithValue("@TinhTrang", phong.TinhTrang);
                    cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log error if needed
                return false;
            }
        }

        // Xóa phòng
        public bool xoaThongTinPhong(Phong phong)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE Phong SET IsDeleted = 1 WHERE SoPhong = @SoPhong";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log error if needed
                return false;
            }
        }

        // Lấy danh sách phòng
        public List<Phong> getPhong()
        {
            List<Phong> lstPhong = new List<Phong>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = @"
                                    SELECT 
                                        p.SoPhong, 
                                        p.MaLoaiPhong, 
                                        p.DonDep, 
                                        lp.TenLoaiPhong AS LoaiPhong 
                                    FROM Phong p
                                    INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                                    WHERE p.IsDeleted = 0";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lstPhong.Add(new Phong
                            {
                                SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
                                MaLoaiPhong = reader.GetInt32(reader.GetOrdinal("MaLoaiPhong")),
                                TinhTrang = reader.GetString(reader.GetOrdinal("DonDep")),
                                LoaiPhong = reader.GetString(reader.GetOrdinal("LoaiPhong")) // Lấy tên loại phòng
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách phòng: " + ex.Message); // Log lỗi nếu cần
            }

            return lstPhong;
        }

		public List<Phong_Custom> filterPhongTheoLoai(string loai)
		{
			List<Phong_Custom> filteredPhongList = new List<Phong_Custom>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                    SELECT 
                        p.SoPhong, 
                        p.TinhTrangDonDep, 
                        lp.TenLoaiPhong, 
                        ct.MaCTPT, 
                        ct.NgayBD, 
                        ct.NgayKT, 
                        ct.TinhTrangThue, 
                        ct.PhieuThue.KhachHang.TenKH, 
                        ct.SoNguoiO
                    FROM Phong p
                    INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                    LEFT JOIN CT_PhieuThue ct ON p.SoPhong = ct.SoPhong
                    WHERE lp.TenLoaiPhong = @LoaiPhong AND p.IsDeleted = 0";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@LoaiPhong", loai);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							filteredPhongList.Add(new Phong_Custom
							{
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								MaPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								TinhTrang = reader.GetString(reader.GetOrdinal("TinhTrangDonDep")),
								LoaiPhong = reader.GetString(reader.GetOrdinal("TenLoaiPhong")),
								TenKH = reader.IsDBNull(reader.GetOrdinal("TenKH")) ? string.Empty : reader.GetString(reader.GetOrdinal("TenKH")),
								NgayDen = reader.IsDBNull(reader.GetOrdinal("NgayBD")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("NgayBD")),
								NgayDi = reader.IsDBNull(reader.GetOrdinal("NgayKT")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("NgayKT")),
								SoNguoi = reader.IsDBNull(reader.GetOrdinal("SoNguoiO")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
								SoNgayO = reader.IsDBNull(reader.GetOrdinal("NgayBD")) || reader.IsDBNull(reader.GetOrdinal("NgayKT")) ? (int?)null :
										  (int)(reader.GetDateTime(reader.GetOrdinal("NgayKT")).Subtract(reader.GetDateTime(reader.GetOrdinal("NgayBD"))).TotalDays) + 1,
								SoGio = reader.IsDBNull(reader.GetOrdinal("NgayBD")) || reader.IsDBNull(reader.GetOrdinal("NgayKT")) ? (int?)null :
										(int)(reader.GetDateTime(reader.GetOrdinal("NgayKT")).Subtract(reader.GetDateTime(reader.GetOrdinal("NgayBD"))).TotalHours)
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi lọc danh sách phòng theo loại: " + ex.Message); // Log lỗi nếu cần
			}

			return filteredPhongList;
		}
	}
}
