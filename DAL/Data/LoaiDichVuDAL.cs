using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class LoaiDichVuDAL
	{
		private static LoaiDichVuDAL instance;

		private LoaiDichVuDAL()
		{
		}

		public static LoaiDichVuDAL GetInstance()
		{
			if (instance == null) instance = new LoaiDichVuDAL();
			return instance;
		}

		// Lấy tất cả loại dịch vụ
		public List<LoaiDV> getData()
		{
			var loaiDichVus = new List<LoaiDV>();
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT * FROM LoaiDV WHERE IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							loaiDichVus.Add(new LoaiDV
							{
								MaLoaiDV = reader.GetInt32(reader.GetOrdinal("MaLoaiDV")),
								TenLoaiDV = reader.GetString(reader.GetOrdinal("TenLoaiDV"))
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return loaiDichVus;
		}

		// Thêm loại dịch vụ mới
		public bool addDataLoaiDV(LoaiDV loaiDV)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        INSERT INTO LoaiDV (TenLoaiDV, IsDeleted)
                        VALUES (@TenLoaiDV, 0)";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiDV", loaiDV.TenLoaiDV);

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

		// Xóa loại dịch vụ
		public bool xoaLoaiDV(LoaiDV loaiDV)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE LoaiDV SET IsDeleted = 1 WHERE MaLoaiDV = @MaLoaiDV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaLoaiDV", loaiDV.MaLoaiDV);

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

		// Cập nhật loại dịch vụ
		public bool capnhatLoaiDV(LoaiDV loaiDV)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        UPDATE LoaiDV 
                        SET TenLoaiDV = @TenLoaiDV
                        WHERE MaLoaiDV = @MaLoaiDV";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiDV", loaiDV.TenLoaiDV);
					cmd.Parameters.AddWithValue("@MaLoaiDV", loaiDV.MaLoaiDV);

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

		// Kiểm tra tên loại dịch vụ có tồn tại không
		public bool KiemTraTenLoaiDichVu(LoaiDV loaiDV)
		{
			var isExist = false;
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM LoaiDV WHERE TenLoaiDV = @TenLoaiDV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiDV", loaiDV.TenLoaiDV);

					conn.Open();
					var count = Convert.ToInt32(cmd.ExecuteScalar());
					if (count > 0) isExist = true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return isExist;
		}

		// Hiển thị lại loại dịch vụ đã xóa
		public bool hienThiLaiLoaiDV(string tenLoaiDV)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE LoaiDV SET IsDeleted = 0 WHERE TenLoaiDV = @TenLoaiDV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiDV", tenLoaiDV);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						Console.WriteLine($"Đã khôi phục loại phòng: {tenLoaiDV}");
						return true;
					}

					Console.WriteLine($"Không tìm thấy loại phòng với tên: {tenLoaiDV}");
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