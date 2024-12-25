using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class PhongDAL
	{
		private static PhongDAL Instance;

		private PhongDAL()
		{
		}

		public static PhongDAL GetInstance()
		{
			if (Instance == null) Instance = new PhongDAL();
			return Instance;
		}

		// Lấy dữ liệu phòng theo ngày
		public ObservableCollection<Phong_Custom> getDataPhongTheoNgay(DateTime? ngayChon)
		{
			var ls = new ObservableCollection<Phong_Custom>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT ct.MaCTPT,
                                    COALESCE(kh.TenKH, '') AS TenKH,
                                    p.SoPhong AS MaPhong,
                                    p.DonDep AS DonDep,
                                    COALESCE(ct.TinhTrangThue, 'Phòng trống') AS TinhTrang,
                                    lp.TenLoaiPhong AS LoaiPhong,
                                    ct.NgayBD AS NgayDen,
                                    CASE 
                                        WHEN ct.NgayBD IS NULL OR ct.NgayKT IS NULL THEN 0
                                        ELSE DATEDIFF(ct.NgayKT, ct.NgayBD) + 1
                                    END AS SoNgayO,
                                    CASE 
                                        WHEN ct.NgayBD IS NULL OR ct.NgayKT IS NULL THEN 0
                                        ELSE TIMESTAMPDIFF(HOUR, ct.NgayBD, ct.NgayKT)
                                    END AS SoGio,
                                    ct.NgayKT AS NgayDi,
                                    COALESCE(ct.SoNguoiO, 0) AS SoNguoi
                                    FROM Phong p
                                    LEFT JOIN CT_PhieuThue ct ON p.SoPhong = ct.SoPhong AND ((@ngayChon BETWEEN ct.NgayBD AND ct.NgayKT)
                                                                                            OR (ct.NgayBD > @ngayChon) 
                                                                                            OR ct.NgayBD IS NULL) 
                                                                                        AND ct.TinhTrangThue <> 'Đã thanh toán'
                                    LEFT JOIN PhieuThue pt ON ct.MaPhieuThue = pt.MaPhieuThue
                                    LEFT JOIN KhachHang kh ON pt.MaKH = kh.MaKH
                                    LEFT JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                                    WHERE p.IsDeleted = 0";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@ngayChon", ngayChon);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							ls.Add(new Phong_Custom
							{
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT"))
									? 0
									: reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								MaPhong = reader.GetString(reader.GetOrdinal("MaPhong")),
								DonDep = reader.GetString(reader.GetOrdinal("DonDep")),
								TinhTrang = reader.GetString(reader.GetOrdinal("TinhTrang")),
								LoaiPhong = reader.GetString(reader.GetOrdinal("LoaiPhong")),
								NgayDen = reader.IsDBNull(reader.GetOrdinal("NgayDen"))
									? (DateTime?)null
									: reader.GetDateTime(reader.GetOrdinal("NgayDen")),
								SoNgayO = reader.GetInt32(reader.GetOrdinal("SoNgayO")),
								SoGio = reader.GetInt32(reader.GetOrdinal("SoGio")),
								NgayDi = reader.IsDBNull(reader.GetOrdinal("NgayDi"))
									? (DateTime?)null
									: reader.GetDateTime(reader.GetOrdinal("NgayDi")),
								SoNguoi = reader.GetInt32(reader.GetOrdinal("SoNguoi"))
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return ls;
		}

		// Lấy dữ liệu phong_custom 
		public ObservableCollection<Phong_Custom> getDataPhong_Custom()
		{
			var ls = new ObservableCollection<Phong_Custom>();
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT ct.MaCTPT,
                                    COALESCE(kh.TenKH, '') AS TenKH,
                                    p.SoPhong AS MaPhong,
                                    p.DonDep AS DonDep,
                                    COALESCE(ct.TinhTrangThue, 'Phòng trống') AS TinhTrang,
                                    lp.TenLoaiPhong AS LoaiPhong,
                                    ct.NgayBD AS NgayDen,
                                    CASE 
                                        WHEN ct.NgayBD IS NULL OR ct.NgayKT IS NULL THEN 0
                                        ELSE DATEDIFF(ct.NgayKT, ct.NgayBD) + 1
                                    END AS SoNgayO,
                                    CASE 
                                        WHEN ct.NgayBD IS NULL OR ct.NgayKT IS NULL THEN 0
                                        ELSE TIMESTAMPDIFF(HOUR, ct.NgayBD, ct.NgayKT)
                                    END AS SoGio,
                                    ct.NgayKT AS NgayDi,
                                    COALESCE(ct.SoNguoiO, 0) AS SoNguoi
                                    FROM Phong p
                                    LEFT JOIN CT_PhieuThue ct ON p.SoPhong = ct.SoPhong
                                    LEFT JOIN PhieuThue pt ON ct.MaPhieuThue = pt.MaPhieuThue
                                    LEFT JOIN KhachHang kh ON pt.MaKH = kh.MaKH
                                    LEFT JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                                    WHERE p.IsDeleted = 0";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							ls.Add(new Phong_Custom
							{
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT"))
									? 0
									: reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								MaPhong = reader.GetString(reader.GetOrdinal("MaPhong")),
								DonDep = reader.GetString(reader.GetOrdinal("DonDep")),
								TinhTrang = reader.GetString(reader.GetOrdinal("TinhTrang")),
								LoaiPhong = reader.GetString(reader.GetOrdinal("LoaiPhong")),
								NgayDen = reader.IsDBNull(reader.GetOrdinal("NgayDen"))
									? (DateTime?)null
									: reader.GetDateTime(reader.GetOrdinal("NgayDen")),
								SoNgayO = reader.GetInt32(reader.GetOrdinal("SoNgayO")),
								SoGio = reader.GetInt32(reader.GetOrdinal("SoGio")),
								NgayDi = reader.IsDBNull(reader.GetOrdinal("NgayDi"))
									? (DateTime?)null
									: reader.GetDateTime(reader.GetOrdinal("NgayDi")),
								SoNguoi = reader.GetInt32(reader.GetOrdinal("SoNguoi"))
							});
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
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE Phong SET DonDep = @DonDep WHERE SoPhong = @MaPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@DonDep", text);
					cmd.Parameters.AddWithValue("@MaPhong", maPhong);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

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
			var connectionString = Properties.Resources.MySqlConnection;

			using (var conn = new MySqlConnection(connectionString))
			{
				var query = "SELECT LoaiPhong.GiaNgay, LoaiPhong.GiaGio FROM Phong p " +
				            "INNER JOIN LoaiPhong ON p.MaLoaiPhong = LoaiPhong.MaLoaiPhong " +
				            "WHERE p.SoPhong = @MaPhong";
				var cmd = new MySqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@MaPhong", maphong);

				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						if (isday)
							return reader.GetDecimal(reader.GetOrdinal("GiaNgay"));
						return reader.GetDecimal(reader.GetOrdinal("GiaGio"));
					}
				}
			}

			return 0; // Default giá tiền
		}

		// Lấy danh sách phòng trống
		public List<PhongTrong> getPhongTrong(DateTime? ngayBD, DateTime? ngayKT)
		{
			var lsPTrong = new List<PhongTrong>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        SELECT p.SoPhong, lp.TenLoaiPhong
                        FROM Phong p
                        JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                        WHERE p.IsDeleted = 0
                        AND p.SoPhong NOT IN (
                            SELECT ct.SoPhong
                            FROM CT_PhieuThue ct
                            WHERE ((ct.NgayBD <= @NgayKT AND ct.NgayKT >= @NgayBD)) AND ct.TinhTrangThue != 'Đã thanh toán'
                        )";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@NgayBD", ngayBD);
					cmd.Parameters.AddWithValue("@NgayKT", ngayKT);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							lsPTrong.Add(new PhongTrong
							{
								SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								TenLoaiPhong = reader.GetString(reader.GetOrdinal("TenLoaiPhong"))
							});
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
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query =
						"INSERT INTO Phong (SoPhong, MaLoaiPhong, DonDep, IsDeleted) VALUES (@SoPhong, @MaLoaiPhong, @DonDep, 0)";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);
					cmd.Parameters.AddWithValue("@DonDep", phong.DonDep);
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
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query =
						"UPDATE Phong SET MaLoaiPhong = @MaLoaiPhong, DonDep = @DonDep WHERE SoPhong = @SoPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);
					cmd.Parameters.AddWithValue("@DonDep", phong.DonDep);
					cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

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
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE Phong SET IsDeleted = 1 WHERE SoPhong = @SoPhong";
					var cmd = new MySqlCommand(query, conn);
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
		public List<Phong> getDataPhong()
		{
			var lstPhong = new List<Phong>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                                    SELECT 
                                        p.SoPhong, 
                                        p.MaLoaiPhong, 
                                        p.DonDep, 
                                        lp.TenLoaiPhong AS LoaiPhong 
                                    FROM Phong p
                                    INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                                    WHERE p.IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							lstPhong.Add(new Phong
							{
								SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								MaLoaiPhong = reader.GetInt32(reader.GetOrdinal("MaLoaiPhong")),
								DonDep = reader.GetString(reader.GetOrdinal("DonDep")),
								LoaiPhong = reader.GetString(reader.GetOrdinal("LoaiPhong")) // Lấy tên loại phòng
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi lấy danh sách phòng: " + ex.Message); // Log lỗi nếu cần
			}

			return lstPhong;
		}

		public Phong getDataPhongTheoSoPhong(string soPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                                    SELECT 
                                        p.SoPhong, 
                                        p.MaLoaiPhong, 
                                        p.DonDep, 
                                        lp.TenLoaiPhong AS LoaiPhong 
                                    FROM Phong p
                                    INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                                    WHERE p.IsDeleted = 0 AND p.SoPhong = @SoPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@SoPhong", soPhong);
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
							return new Phong
							{
								SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								MaLoaiPhong = reader.GetInt32(reader.GetOrdinal("MaLoaiPhong")),
								DonDep = reader.GetString(reader.GetOrdinal("DonDep")),
								LoaiPhong = reader.GetString(reader.GetOrdinal("LoaiPhong")) // Lấy tên loại phòng
							};
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi lấy danh sách phòng: " + ex.Message); // Log lỗi nếu cần
			}

			return null;
		}

		// Hiển thị lại phòng đã xóa
		public bool hienThiLaiPhong(string soPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE Phong SET IsDeleted = 0 WHERE SoPhong = @SoPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@SoPhong", soPhong);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						Console.WriteLine($"Đã khôi phục phòng: {soPhong}");
						return true;
					}

					Console.WriteLine($"Không tìm thấy loại phòng với tên: {soPhong}");
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}
	}
}