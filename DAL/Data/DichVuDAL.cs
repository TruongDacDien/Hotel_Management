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
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
				SELECT 
					dv.MaDV, dv.TenDV, dv.MoTa, dv.MaLoaiDV, dv.Gia, dv.SoLuong, dv.ImageId, dv.ImageURL,
					ldv.TenLoaiDV AS LoaiDV
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
								MaDV = reader.GetInt32("MaDV"),
								TenDV = reader.GetString("TenDV"),
								MoTa = reader.GetString("MoTa"),
								MaLoaiDV = reader.GetInt32("MaLoaiDV"),
								Gia = reader.GetDecimal("Gia"),
								SoLuong = reader.GetInt32("SoLuong"),
								ImageId = reader["ImageId"]?.ToString(),
								ImageURL = reader["ImageURL"]?.ToString(),
								TenLoaiDV = reader.GetString("LoaiDV")
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return lsNDVCT;
		}

		// Lấy danh sách tất cả dịch vụ
		public List<DichVu> getDataDichVu()
		{
			var lsDichVu = new List<DichVu>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT 
					dv.MaDV, dv.TenDV, dv.MoTa, dv.MaLoaiDV, dv.Gia, dv.SoLuong, dv.ImageId, dv.ImageURL,
					ldv.TenLoaiDV AS LoaiDV
				FROM DichVu dv
				INNER JOIN LoaiDV ldv ON dv.MaLoaiDV = ldv.MaLoaiDV
				WHERE dv.IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							lsDichVu.Add(new DichVu
							{
								MaDV = reader.GetInt32("MaDV"),
								TenDV = reader.GetString("TenDV"),
								MoTa = reader.GetString("MoTa"),
								MaLoaiDV = reader.GetInt32("MaLoaiDV"),
								Gia = reader.GetDecimal("Gia"),
								SoLuong = reader.GetInt32("SoLuong"),
								ImageId = reader["ImageId"]?.ToString(),
								ImageURL = reader["ImageURL"]?.ToString(),
								TenLoaiDV = reader.GetString("LoaiDV")
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
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					conn.Open();

					var query = @"INSERT INTO DichVu (TenDV, MoTa, MaLoaiDV, Gia, SoLuong, ImageId, ImageURL, IsDeleted)
								VALUES (@TenDV, @MoTa, @MaLoaiDV, @Gia, @SoLuong, @ImageId, @ImageURL, 0);
								SELECT LAST_INSERT_ID();";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
					cmd.Parameters.AddWithValue("@MoTa", dv.MoTa);
					cmd.Parameters.AddWithValue("@MaLoaiDV", dv.MaLoaiDV);
					cmd.Parameters.AddWithValue("@Gia", dv.Gia);
					cmd.Parameters.AddWithValue("@SoLuong", dv.SoLuong);
					cmd.Parameters.AddWithValue("@ImageId", dv.ImageId ?? "hotel_management/image_default");
					cmd.Parameters.AddWithValue("@ImageURL", dv.ImageURL ?? "https://res.cloudinary.com/dzaoyffio/image/upload/v1750100245/image_default.png");

					object result = cmd.ExecuteScalar();
					if (result == null || result == DBNull.Value)
						throw new Exception("Không thể lấy mã dịch vụ sau khi thêm.");

					dv.MaDV = Convert.ToInt32(result);

					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi thêm dịch vụ: " + ex.Message);
				return false;
			}
		}

		// Cập nhật dịch vụ
		public bool capNhatDichVu(DichVu dv)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
				UPDATE DichVu
				SET TenDV = @TenDV, MoTa = @MoTa, MaLoaiDV = @MaLoaiDV, Gia = @Gia, SoLuong = @SoLuong, ImageId = @ImageId, ImageURL = @ImageURL
				WHERE MaDV = @MaDV";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaDV", dv.MaDV);
					cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
					cmd.Parameters.AddWithValue("@MoTa", dv.MoTa);
					cmd.Parameters.AddWithValue("@MaLoaiDV", dv.MaLoaiDV);
					cmd.Parameters.AddWithValue("@Gia", dv.Gia);
					cmd.Parameters.AddWithValue("@SoLuong", dv.SoLuong);
					cmd.Parameters.AddWithValue("@ImageId", dv.ImageId ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@ImageURL", dv.ImageURL ?? (object)DBNull.Value);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		// Xóa dịch vụ
		public bool xoaDichVu(DichVu dv)
		{
			var connectionString = Properties.Resources.MySqlConnection;
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
			var connectionString = Properties.Resources.MySqlConnection;
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
			var connectionString = Properties.Resources.MySqlConnection;

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

		public void capNhatHinhAnhDichVu(int maDV, string imageId, string imageUrl)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			using (var conn = new MySqlConnection(connectionString))
			{
				conn.Open();
				var query = @"UPDATE DichVu 
							SET ImageId = @ImageId, ImageURL = @ImageURL 
							WHERE MaDV = @MaDV";
				var cmd = new MySqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@ImageId", imageId);
				cmd.Parameters.AddWithValue("@ImageURL", imageUrl);
				cmd.Parameters.AddWithValue("@MaDV", maDV);
				cmd.ExecuteNonQuery();
			}
		}
	}
}