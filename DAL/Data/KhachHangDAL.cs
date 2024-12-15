using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class KhachHangDAL
	{
		private static KhachHangDAL Instance;

		private KhachHangDAL()
		{
		}

		public static KhachHangDAL GetInstance()
		{
			if (Instance == null) Instance = new KhachHangDAL();
			return Instance;
		}

		// Thêm khách hàng mới
		public bool addKhachHang(KhachHang kh, out string error)
		{
			error = string.Empty;
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        INSERT INTO KhachHang (TenKH, GioiTinh, CCCD, DiaChi, SDT, QuocTich, IsDeleted)
                        VALUES (@TenKH, @GioiTinh, @CCCD, @DiaChi, @SDT, @QuocTich, 0)";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenKH", kh.TenKH);
					cmd.Parameters.AddWithValue("@GioiTinh", kh.GioiTinh);
					cmd.Parameters.AddWithValue("@CCCD", kh.CCCD);
					cmd.Parameters.AddWithValue("@DiaChi", kh.DiaChi);
					cmd.Parameters.AddWithValue("@SDT", kh.SDT);
					cmd.Parameters.AddWithValue("@QuocTich", kh.QuocTich);

					conn.Open();
					cmd.ExecuteNonQuery();
				}

				return true;
			}
			catch (Exception ex)
			{
				error = ex.Message;
				return false;
			}
		}

		// Kiểm tra tồn tại khách hàng dựa trên CCCD
		public KhachHang kiemTraTonTaiKhachHang(string CCCD)
		{
			KhachHang khachHang = null;
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT * FROM KhachHang WHERE CCCD = @CCCD";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@CCCD", CCCD);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
							khachHang = new KhachHang
							{
								MaKH = reader.GetInt32(reader.GetOrdinal("MaKH")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
								CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
								DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
								SDT = reader.GetString(reader.GetOrdinal("SDT")),
								QuocTich = reader.GetString(reader.GetOrdinal("QuocTich")),
								IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
							};
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return khachHang;
		}

		// Lấy tất cả khách hàng
		public List<KhachHang> getData()
		{
			var lstKhachHang = new List<KhachHang>();
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT * FROM KhachHang WHERE IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							lstKhachHang.Add(new KhachHang
							{
								MaKH = reader.GetInt32(reader.GetOrdinal("MaKH")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
								CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
								DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
								SDT = reader.GetString(reader.GetOrdinal("SDT")),
								QuocTich = reader.GetString(reader.GetOrdinal("QuocTich"))
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return lstKhachHang;
		}

		// Cập nhật thông tin khách hàng
		public bool capnhatKhachHang(KhachHang khachHang)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			var error = string.Empty;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        UPDATE KhachHang 
                        SET TenKH = @TenKH, CCCD = @CCCD, GioiTinh = @GioiTinh, DiaChi = @DiaChi, SDT = @SDT, QuocTich = @QuocTich
                        WHERE MaKH = @MaKH";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenKH", khachHang.TenKH);
					cmd.Parameters.AddWithValue("@CCCD", khachHang.CCCD);
					cmd.Parameters.AddWithValue("@GioiTinh", khachHang.GioiTinh);
					cmd.Parameters.AddWithValue("@DiaChi", khachHang.DiaChi);
					cmd.Parameters.AddWithValue("@SDT", khachHang.SDT);
					cmd.Parameters.AddWithValue("@QuocTich", khachHang.QuocTich);
					cmd.Parameters.AddWithValue("@MaKH", khachHang.MaKH);

					conn.Open();
					cmd.ExecuteNonQuery();
				}

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Xóa khách hàng
		public bool xoaKhachHang(KhachHang khachHang)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE KhachHang SET IsDeleted = 1 WHERE MaKH = @MaKH";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaKH", khachHang.MaKH);

					conn.Open();
					cmd.ExecuteNonQuery();
				}

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Lấy tên khách hàng theo mã phiếu thuê
		public string layTenKhachHangTheoMaPT(int? maPhieuThue)
		{
			var tenKhachHang = string.Empty;
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query =
						"SELECT TenKH FROM KhachHang WHERE MaKH = (SELECT MaKH FROM PhieuThue WHERE MaPhieuThue = @MaPhieuThue)";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read()) tenKhachHang = reader.GetString(reader.GetOrdinal("TenKH"));
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return tenKhachHang;
		}

		public int layMaKHMoiNhat()
		{
			var maKH = -1; // Giá trị mặc định nếu không tìm thấy
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT MAX(MaKH) AS MaKH FROM KhachHang";
					var cmd = new MySqlCommand(query, conn);

					conn.Open();
					var result = cmd.ExecuteScalar();

					if (result != null && result != DBNull.Value) maKH = Convert.ToInt32(result);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi lấy MaKH mới nhất: " + ex.Message);
			}

			return maKH;
		}

		// Hiển thị lại khách hàng đã xóa
		public bool hienThiLaiKhachHang(string cCCD)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE KhachHang SET IsDeleted = 0 WHERE CCCD = @CCCD";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@CCCD", cCCD);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						Console.WriteLine($"Đã khôi phục loại phòng: {cCCD}");
						return true;
					}

					Console.WriteLine($"Không tìm thấy loại phòng với tên: {cCCD}");
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