using DAL.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;

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
			var connectionString = Properties.Resources.MySqlConnection;

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
							TenTN = reader.GetString(reader.GetOrdinal("TenTN")),
							SoLuong = reader.GetInt32(reader.GetOrdinal("SoLuong")),
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
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"INSERT INTO TienNghi (TenTN, SoLuong,IsDeleted)
                          VALUES (@TenTN, @ImageId, @ImageURL, @SoLuong,0)";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tn.TenTN);
					cmd.Parameters.AddWithValue("@SoLuong", tn.SoLuong);

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
			var connectionString = Properties.Resources.MySqlConnection;
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
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"UPDATE TienNghi 
                          SET TenTN = @TenTN, SoLuong = @SoLuong 
                          WHERE MaTN = @MaTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tn.TenTN);
					cmd.Parameters.AddWithValue("@SoLuong", tn.SoLuong);
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
			var connectionString = Properties.Resources.MySqlConnection;
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
			var connectionString = Properties.Resources.MySqlConnection;

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

		public bool capNhatSoLuongTienNghi(int maTN, int soLuongThayDoi)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TienNghi SET SoLuong = SoLuong + @SoLuongThayDoi WHERE MaTN = @MaTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@SoLuongThayDoi", soLuongThayDoi);
					cmd.Parameters.AddWithValue("@MaTN", maTN);
					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					if (rowsAffected > 0)
					{
						return true;
					}
					return false;
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Lỗi khi cập nhật số lượng tiện nghi: {ex.Message}");
				return false;
			}
		}
	}
}