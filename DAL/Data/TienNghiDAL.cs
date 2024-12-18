using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class TienNghiDAL
	{
		private static TienNghiDAL instance;

		private TienNghiDAL()
		{
		}

		public static TienNghiDAL GetInstance()
		{
			if (instance == null) instance = new TienNghiDAL();
			return instance;
		}

		// Lấy tất cả dữ liệu tiện nghi từ cơ sở dữ liệu
		public List<TienNghi> getData()
		{
			var tienNghiList = new List<TienNghi>();
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			using (var conn = new MySqlConnection(connectionString))
			{
				var query = "SELECT * FROM TienNghi WHERE IsDeleted = 0";
				var cmd = new MySqlCommand(query, conn);
				conn.Open();

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var tn = new TienNghi
						{
							MaTN = reader.GetInt32(reader.GetOrdinal("MaTN")),
							TenTN = reader.GetString(reader.GetOrdinal("TenTN"))
						};
						tienNghiList.Add(tn);
					}
				}
			}

			return tienNghiList;
		}

		// Thêm tiện nghi mới vào cơ sở dữ liệu
		public bool addTienNghi(TienNghi tn)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "INSERT INTO TienNghi (TenTN, IsDeleted) VALUES (@TenTN, 0)";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tn.TenTN);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		// Xóa tiện nghi theo MaTN
		public bool xoaTienNghi(TienNghi tn)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TienNghi SET IsDeleted = 1 WHERE MaTN = @MaTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", tn.MaTN);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		// Cập nhật thông tin tiện nghi
		public bool capnhatTienNghi(TienNghi tn)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TienNghi SET TenTN = @TenTN WHERE MaTN = @MaTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tn.TenTN);
					cmd.Parameters.AddWithValue("@MaTN", tn.MaTN);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		// Kiểm tra tên tiện nghi đã tồn tại hay chưa
		public bool KiemTraTenTienNghi(TienNghi tn)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM TienNghi WHERE TenTN = @TenTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tn.TenTN);

					conn.Open();
					var count = Convert.ToInt32(cmd.ExecuteScalar());

					return count > 0;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		// Hiển thị lại tiện nghi đã xóa
		public bool hienThiLaiTienNghi(string tenTN)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TienNghi SET IsDeleted = 0 WHERE TenTN = @TenTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tenTN);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						Console.WriteLine($"Đã khôi phục tiện nghi: {tenTN}");
						return true;
					}

					Console.WriteLine($"Không tìm thấy loại phòng với tên: {tenTN}");
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