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
					var query = @"
                SELECT ct.MaCTPT,
                       COALESCE(kh.TenKH, '') AS TenKH,
                       p.SoPhong AS MaPhong,
                       p.DonDep AS DonDep,
                       COALESCE(ct.TinhTrangThue, 'Phòng trống') AS TinhTrang,
                       lp.TenLoaiPhong AS LoaiPhong,
                       ct.NgayBD AS NgayDen,
                       CASE 
                           WHEN ct.NgayBD IS NULL OR ct.NgayKT IS NULL THEN 0
                           ELSE CEIL(DATEDIFF(ct.NgayKT, ct.NgayBD))
                       END AS SoNgayO,
                       CASE 
                           WHEN ct.NgayBD IS NULL OR ct.NgayKT IS NULL THEN 0
                           ELSE TIMESTAMPDIFF(HOUR, ct.NgayBD, ct.NgayKT)
                       END AS SoGio,
                       ct.NgayKT AS NgayDi,
                       COALESCE(ct.SoNguoiO, 0) AS SoNguoi
                FROM Phong p
                LEFT JOIN CT_PhieuThue ct ON p.SoPhong = ct.SoPhong 
                    AND @ngayChon BETWEEN ct.NgayBD AND ct.NgayKT 
                    AND ct.TinhTrangThue <> 'Đã thanh toán'
                LEFT JOIN PhieuThue pt ON ct.MaPhieuThue = pt.MaPhieuThue AND pt.IsDeleted = 0
                LEFT JOIN KhachHang kh ON pt.MaKH = kh.MaKH AND kh.IsDeleted = 0
                LEFT JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong AND lp.IsDeleted = 0
                WHERE p.IsDeleted = 0";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@ngayChon", ngayChon ?? DateTime.Now);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							ls.Add(new Phong_Custom
							{
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT")) ? 0 : reader.GetInt32("MaCTPT"),
								TenKH = reader.GetString("TenKH"),
								MaPhong = reader.GetString("MaPhong"),
								DonDep = reader.GetString("DonDep"),
								TinhTrang = reader.GetString("TinhTrang"),
								LoaiPhong = reader.GetString("LoaiPhong"),
								NgayDen = reader.IsDBNull(reader.GetOrdinal("NgayDen")) ? null : reader.GetDateTime("NgayDen"),
								SoNgayO = reader.GetInt32("SoNgayO"),
								SoGio = reader.GetDecimal("SoGio"), // Sửa thành decimal
								NgayDi = reader.IsDBNull(reader.GetOrdinal("NgayDi")) ? null : reader.GetDateTime("NgayDi"),
								SoNguoi = reader.GetInt32("SoNguoi")
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi lấy danh sách phòng theo ngày: {ex.Message}\nStackTrace: {ex.StackTrace}");
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
					var query = @"
                SELECT ct.MaCTPT,
                       COALESCE(kh.TenKH, '') AS TenKH,
                       p.SoPhong AS MaPhong,
                       p.DonDep AS DonDep,
                       COALESCE(ct.TinhTrangThue, 'Phòng trống') AS TinhTrang,
                       lp.TenLoaiPhong AS LoaiPhong,
                       ct.NgayBD AS NgayDen,
                       CASE 
                           WHEN ct.NgayBD IS NULL OR ct.NgayKT IS NULL THEN 0
                           ELSE CEIL(DATEDIFF(ct.NgayKT, ct.NgayBD))
                       END AS SoNgayO,
                       CASE 
                           WHEN ct.NgayBD IS NULL OR ct.NgayKT IS NULL THEN 0
                           ELSE TIMESTAMPDIFF(HOUR, ct.NgayBD, ct.NgayKT)
                       END AS SoGio,
                       ct.NgayKT AS NgayDi,
                       COALESCE(ct.SoNguoiO, 0) AS SoNguoi
                FROM Phong p
                LEFT JOIN CT_PhieuThue ct ON p.SoPhong = ct.SoPhong AND ct.TinhTrangThue <> 'Đã thanh toán'
                LEFT JOIN PhieuThue pt ON ct.MaPhieuThue = pt.MaPhieuThue AND pt.IsDeleted = 0
                LEFT JOIN KhachHang kh ON pt.MaKH = kh.MaKH AND kh.IsDeleted = 0
                LEFT JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong AND lp.IsDeleted = 0
                WHERE p.IsDeleted = 0";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							ls.Add(new Phong_Custom
							{
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT")) ? 0 : reader.GetInt32("MaCTPT"),
								TenKH = reader.GetString("TenKH"),
								MaPhong = reader.GetString("MaPhong"),
								DonDep = reader.GetString("DonDep"),
								TinhTrang = reader.GetString("TinhTrang"),
								LoaiPhong = reader.GetString("LoaiPhong"),
								NgayDen = reader.IsDBNull(reader.GetOrdinal("NgayDen")) ? null : reader.GetDateTime("NgayDen"),
								SoNgayO = reader.GetInt32("SoNgayO"),
								SoGio = reader.GetDecimal("SoGio"),
								NgayDi = reader.IsDBNull(reader.GetOrdinal("NgayDi")) ? null : reader.GetDateTime("NgayDi"),
								SoNguoi = reader.GetInt32("SoNguoi")
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi lấy danh sách phòng: {ex.Message}\nStackTrace: {ex.StackTrace}");
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
				if (string.IsNullOrEmpty(maPhong) || string.IsNullOrEmpty(text) || text.Length > 80)
				{
					error = "Số phòng hoặc trạng thái không hợp lệ.";
					return false;
				}

				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE Phong SET DonDep = @DonDep WHERE SoPhong = @MaPhong AND IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@DonDep", text);
					cmd.Parameters.AddWithValue("@MaPhong", maPhong);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected == 0)
					{
						error = $"Không tìm thấy phòng có số phòng: {maPhong} hoặc phòng đã bị xóa.";
						return false;
					}

					return true;
				}
			}
			catch (Exception ex)
			{
				error = $"Lỗi khi cập nhật trạng thái phòng: {ex.Message}";
				return false;
			}
		}

		// Lấy giá tiền của phòng theo mã phòng
		public decimal layGiaTienTheoMaPhong(string maPhong, bool isDay)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                SELECT lp.GiaNgay, lp.GiaGio 
                FROM Phong p 
                INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong 
                WHERE p.SoPhong = @MaPhong AND p.IsDeleted = 0 AND lp.IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhong", maPhong);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							return isDay ? reader.GetDecimal("GiaNgay") : reader.GetDecimal("GiaGio");
						}
					}

					throw new Exception($"Không tìm thấy phòng có số phòng: {maPhong} hoặc phòng/loại phòng đã bị xóa.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi lấy giá phòng: {ex.Message}");
				return 0; // Giá trị mặc định
			}
		}

		// Lấy danh sách phòng trống
		public List<PhongTrong> getPhongTrong(DateTime? ngayBD, DateTime? ngayKT)
		{
			var lsPTrong = new List<PhongTrong>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				if (!ngayBD.HasValue || !ngayKT.HasValue || ngayBD > ngayKT)
				{
					throw new ArgumentException("Ngày bắt đầu và ngày kết thúc không hợp lệ.");
				}

				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                SELECT p.SoPhong, lp.TenLoaiPhong
                FROM Phong p
                INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                WHERE p.IsDeleted = 0 AND lp.IsDeleted = 0 
                    AND p.DonDep NOT IN ('Sửa chữa', 'Chưa dọn dẹp')
                    AND p.SoPhong NOT IN (
                        SELECT ct.SoPhong
                        FROM CT_PhieuThue ct
                        WHERE ct.NgayBD <= @NgayKT AND ct.NgayKT >= @NgayBD 
                            AND ct.TinhTrangThue != 'Đã thanh toán'
                    )";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@NgayBD", ngayBD);
					cmd.Parameters.AddWithValue("@NgayKT", ngayKT);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							lsPTrong.Add(new PhongTrong
							{
								SoPhong = reader.GetString("SoPhong"),
								TenLoaiPhong = reader.GetString("TenLoaiPhong")
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi lấy danh sách phòng trống: {ex.Message}\nStackTrace: {ex.StackTrace}");
			}

			return lsPTrong;
		}

		// Thêm mới phòng
		public bool addDataPhong(Phong phong)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				if (string.IsNullOrEmpty(phong.SoPhong) || phong.DonDep?.Length > 80)
				{
					throw new ArgumentException("Số phòng hoặc trạng thái dọn dẹp không hợp lệ.");
				}

				// Kiểm tra MaLoaiPhong tồn tại
				if (!KiemTraMaLoaiPhongTonTai(phong.MaLoaiPhong))
				{
					throw new ArgumentException($"Loại phòng với MaLoaiPhong {phong.MaLoaiPhong} không tồn tại hoặc đã bị xóa.");
				}

				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                INSERT INTO Phong (SoPhong, MaLoaiPhong, DonDep, IsSelected, IsDeleted) 
                VALUES (@SoPhong, @MaLoaiPhong, @DonDep, 0, 0)";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);
					cmd.Parameters.AddWithValue("@DonDep", phong.DonDep ?? "Đã dọn dẹp");
					conn.Open();
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch (MySqlException ex) when (ex.Number == 1062) // Duplicate key
			{
				Console.WriteLine($"Lỗi: Số phòng '{phong.SoPhong}' đã tồn tại.");
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi thêm phòng: {ex.Message}\nStackTrace: {ex.StackTrace}");
				return false;
			}
		}

		// Thêm phương thức kiểm tra MaLoaiPhong
		private bool KiemTraMaLoaiPhongTonTai(int maLoaiPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM LoaiPhong WHERE MaLoaiPhong = @MaLoaiPhong AND IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", maLoaiPhong);
					conn.Open();
					return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi kiểm tra MaLoaiPhong: {ex.Message}");
				return false;
			}
		}

		// Cập nhật phòng
		public bool capNhatPhong(Phong phong)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				if (string.IsNullOrEmpty(phong.SoPhong) || phong.DonDep?.Length > 80)
				{
					throw new ArgumentException("Số phòng hoặc trạng thái dọn dẹp không hợp lệ.");
				}

				if (!KiemTraMaLoaiPhongTonTai(phong.MaLoaiPhong))
				{
					throw new ArgumentException($"Loại phòng với MaLoaiPhong {phong.MaLoaiPhong} không tồn tại hoặc đã bị xóa.");
				}

				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                UPDATE Phong 
                SET MaLoaiPhong = @MaLoaiPhong, DonDep = @DonDep 
                WHERE SoPhong = @SoPhong AND IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", phong.MaLoaiPhong);
					cmd.Parameters.AddWithValue("@DonDep", phong.DonDep ?? "Đã dọn dẹp");
					cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi cập nhật phòng: {ex.Message}\nStackTrace: {ex.StackTrace}");
				return false;
			}
		}

		// Xóa phòng
		public bool xoaThongTinPhong(Phong phong)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				if (string.IsNullOrEmpty(phong.SoPhong))
				{
					throw new ArgumentException("Số phòng không hợp lệ.");
				}

				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE Phong SET IsDeleted = 1 WHERE SoPhong = @SoPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@SoPhong", phong.SoPhong);
					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi xóa phòng: {ex.Message}\nStackTrace: {ex.StackTrace}");
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
                SELECT p.SoPhong, p.MaLoaiPhong, p.DonDep, lp.TenLoaiPhong AS LoaiPhong 
                FROM Phong p
                INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                WHERE p.IsDeleted = 0 AND lp.IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							lstPhong.Add(new Phong
							{
								SoPhong = reader.GetString("SoPhong"),
								MaLoaiPhong = reader.GetInt32("MaLoaiPhong"),
								DonDep = reader.GetString("DonDep"),
								LoaiPhong = reader.GetString("LoaiPhong")
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi lấy danh sách phòng: {ex.Message}\nStackTrace: {ex.StackTrace}");
			}

			return lstPhong;
		}

		public Phong getDataPhongTheoSoPhong(string soPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				if (string.IsNullOrEmpty(soPhong))
				{
					throw new ArgumentException("Số phòng không hợp lệ.");
				}

				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                SELECT p.SoPhong, p.MaLoaiPhong, p.DonDep, lp.TenLoaiPhong AS LoaiPhong 
                FROM Phong p
                INNER JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
                WHERE p.IsDeleted = 0 AND lp.IsDeleted = 0 AND p.SoPhong = @SoPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@SoPhong", soPhong);
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							return new Phong
							{
								SoPhong = reader.GetString("SoPhong"),
								MaLoaiPhong = reader.GetInt32("MaLoaiPhong"),
								DonDep = reader.GetString("DonDep"),
								LoaiPhong = reader.GetString("LoaiPhong")
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi lấy thông tin phòng: {ex.Message}\nStackTrace: {ex.StackTrace}");
			}

			return null;
		}

		// Hiển thị lại phòng đã xóa
		public bool hienThiLaiPhong(string soPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				if (string.IsNullOrEmpty(soPhong))
				{
					throw new ArgumentException("Số phòng không hợp lệ.");
				}

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

					Console.WriteLine($"Không tìm thấy phòng với số: {soPhong}");
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi khôi phục phòng: {ex.Message}\nStackTrace: {ex.StackTrace}");
				return false;
			}
		}
	}
}