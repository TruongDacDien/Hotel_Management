using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class DichVuDAL
	{
		private static DichVuDAL Instance;

		private DichVuDAL()
		{
		}

		public static DichVuDAL GetInstance()
		{
			if (Instance == null) Instance = new DichVuDAL();
			return Instance;
		}

		// Lấy danh sách dịch vụ (Custom)
		public List<DichVu> getDataDichVu_Custom()
		{
			var lsNDVCT = new List<DichVu>();
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        SELECT 
                            dv.MaDV,dv.MaLoaiDV, dv.TenDV, dv.Gia, ldv.TenLoaiDV AS LoaiDV
                        FROM DichVu dv
                        INNER JOIN LoaiDV ldv ON dv.MaLoaiDV = ldv.MaLoaiDV
						WHERE dv.IsDeleted = 0";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							lsNDVCT.Add(new DichVu
							{
								MaDV = reader.GetInt32(reader.GetOrdinal("MaDV")),
								MaLoaiDV = reader.GetInt32(reader.GetOrdinal("MaLoaiDV")),
								TenDV = reader.GetString(reader.GetOrdinal("TenDV")),
								Gia = reader.GetDecimal(reader.GetOrdinal("Gia")),
								TenLoaiDV = reader.GetString(reader.GetOrdinal("LoaiDV"))
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return lsNDVCT;
		}

		// Lấy danh sách tất cả dịch vụ
		public List<DichVu> getDataDichVu()
		{
			var lsDichVu = new List<DichVu>();
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT dv.MaDV, dv.TenDV, ldv.MaLoaiDV, dv.Gia, ldv.TenLoaiDV
									FROM DichVu dv
									JOIN LoaiDV ldv ON dv.MaLoaiDV = ldv.MaLoaiDV
									WHERE dv.IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							lsDichVu.Add(new DichVu
							{
								MaDV = reader.GetInt32(reader.GetOrdinal("MaDV")),
								TenDV = reader.GetString(reader.GetOrdinal("TenDV")),
								MaLoaiDV = reader.GetInt32(reader.GetOrdinal("MaLoaiDV")),
								TenLoaiDV = reader.GetString(reader.GetOrdinal("TenLoaiDV")),
								Gia = reader.GetDecimal(reader.GetOrdinal("Gia"))
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return lsDichVu;
		}

		// Thêm dịch vụ
		public bool addDichVu(DichVu dv)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query =
						"INSERT INTO DichVu (TenDV, MaLoaiDV, Gia, IsDeleted) VALUES (@TenDV, @MaLoaiDV, @Gia, 0)";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
					cmd.Parameters.AddWithValue("@MaLoaiDV", dv.MaLoaiDV);
					cmd.Parameters.AddWithValue("@Gia", dv.Gia);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Cập nhật dịch vụ
		public bool capNhatDichVu(DichVu dv)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			Console.WriteLine($"MaDV: {dv.MaDV}, TenDV: {dv.TenDV}, MaLoaiDV: {dv.MaLoaiDV}, Gia: {dv.Gia}");
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE DichVu SET TenDV = @TenDV, MaLoaiDV = @MaLoaiDV, Gia = @Gia WHERE MaDV = @MaDV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaDV", dv.MaDV);
					cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
					cmd.Parameters.AddWithValue("@MaLoaiDV", dv.MaLoaiDV);
					cmd.Parameters.AddWithValue("@Gia", Convert.ToDecimal(dv.Gia));

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Xóa dịch vụ
		public bool xoaDichVu(DichVu dv)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE DichVu SET IsDeleted = 1 WHERE MaDV = @MaDV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaDV", dv.MaDV);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Kiểm tra dịch vụ có tên trùng không
		public bool KiemTraTrungTen(DichVu dv)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM DichVu WHERE TenDV = @TenDV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);

					conn.Open();
					var count = Convert.ToInt32(cmd.ExecuteScalar());
					return count > 0; // Trả về true nếu tên dịch vụ đã tồn tại
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Hiển thị lại dịch vụ đã xóa
		public bool hienThiLaiDichVu(string tenDichVu)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE DichVu SET IsDeleted = 0 WHERE TenDV = @TenDV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenDV", tenDichVu);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0) return true;

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