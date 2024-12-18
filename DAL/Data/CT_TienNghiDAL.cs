using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class CT_TienNghiDAL
	{
		private static CT_TienNghiDAL instance;

		private CT_TienNghiDAL()
		{
		}

		public static CT_TienNghiDAL GetInstance()
		{
			if (instance == null) instance = new CT_TienNghiDAL();
			return instance;
		}

		// Lấy danh sách các chi tiết tiện nghi
		public List<CT_TienNghi> getData()
		{
			var listCTTienNghi = new List<CT_TienNghi>();
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
					SELECT CT.MaCTTN, CT.SoPhong, CT.SL, TN.TenTN, CT.MaTN, CT.IsDeleted
					FROM CT_TienNghi CT
					JOIN TienNghi TN ON CT.MaTN = TN.MaTN
					WHERE CT.IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var ctTienNghi = new CT_TienNghi
							{
								MaCTTN = reader.GetInt32(reader.GetOrdinal("MaCTTN")),
								MaTN = reader.GetInt32(reader.GetOrdinal("MaTN")),
								SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								SL = reader.GetInt32(reader.GetOrdinal("SL")),
								TenTN = reader.GetString(reader.GetOrdinal("TenTN")),
								IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
							};
							listCTTienNghi.Add(ctTienNghi);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Xử lý lỗi nếu cần
			}

			return listCTTienNghi;
		}

		// Thêm một chi tiết tiện nghi mới
		public bool addCTTienNghi(CT_TienNghi chiTietTN)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
					INSERT INTO CT_TienNghi (MaTN, SoPhong, SL, TenTN, IsDeleted)
					SELECT @MaTN, @SoPhong, @SL, tn.TenTN, 0
					FROM TienNghi tn
					WHERE tn.MaTN = @MaTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@SoPhong", chiTietTN.SoPhong);
					cmd.Parameters.AddWithValue("@SL", chiTietTN.SL);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Xử lý lỗi nếu cần
				return false;
			}
		}


		// Xóa chi tiết tiện nghi
		public bool xoaCTTienNghi(CT_TienNghi chiTietTN)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE CT_TienNghi SET IsDeleted = 1 WHERE MaCTTN = @MaCTTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTTN", chiTietTN.MaCTTN);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Xử lý lỗi nếu cần
				return false;
			}
		}

		// Cập nhật chi tiết tiện nghi
		public bool capnhatCTTienNghi(CT_TienNghi chiTietTN)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				Console.WriteLine(chiTietTN.MaCTTN + " " + chiTietTN.MaTN + " " + chiTietTN.SoPhong + " " +
				                  chiTietTN.SL + " " + chiTietTN.TenTN);
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
					UPDATE CT_TienNghi
					SET 
						MaTN = @MaTN,
						SoPhong = @SoPhong,
						SL = @SL,
						TenTN = @TenTN
					WHERE MaCTTN = @MaCTTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTTN", chiTietTN.MaCTTN);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@SoPhong", chiTietTN.SoPhong);
					cmd.Parameters.AddWithValue("@SL", chiTietTN.SL);
					cmd.Parameters.AddWithValue("@TenTN", chiTietTN.TenTN);

					conn.Open();

					var rowsAffected = cmd.ExecuteNonQuery();

					Console.WriteLine($"Rows affected: {rowsAffected}");

					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi: {ex.Message}");
				Console.WriteLine($"Stack Trace: {ex.StackTrace}");
				return false;
			}
		}


		// Kiểm tra chi tiết tiện nghi có tồn tại hay không
		public bool KiemTraTonTai(CT_TienNghi chiTietTN)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(1) FROM CT_TienNghi WHERE MaTN = @MaTN AND SoPhong = @SoPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@SoPhong", chiTietTN.SoPhong);

					conn.Open();
					var count = Convert.ToInt32(cmd.ExecuteScalar());

					return count > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Xử lý lỗi nếu cần
				return false;
			}
		}

		// Hiển thị lại chi tiết tiện nghi đã xóa
		public bool hienThiLaiCT_TienNghi(int maTN, string soPhong)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE CT_TienNghi SET IsDeleted = 0 WHERE MaTN = @MaTN AND SoPhong = @SoPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", maTN);
					cmd.Parameters.AddWithValue("@SoPhong", soPhong);

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