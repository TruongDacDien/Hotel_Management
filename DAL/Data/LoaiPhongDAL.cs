using DAL.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Transactions;

namespace DAL.Data
{
	public class LoaiPhongDAL
	{
		private static LoaiPhongDAL instance;

		private LoaiPhongDAL()
		{
		}

		public static LoaiPhongDAL GetInstance()
		{
			if (instance == null) instance = new LoaiPhongDAL();
			return instance;
		}

		// Lấy tất cả loại phòng
		public List<LoaiPhong> getDataLoaiPhong()
		{
			var loaiPhongs = new List<LoaiPhong>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT * FROM LoaiPhong WHERE IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							loaiPhongs.Add(new LoaiPhong
							{
								MaLoaiPhong = reader.GetInt32("MaLoaiPhong"),
								TenLoaiPhong = reader.GetString("TenLoaiPhong"),
								MoTa = reader["MoTa"].ToString(),
								ChinhSach = reader["ChinhSach"].ToString(),
								ChinhSachHuy = reader["ChinhSachHuy"].ToString(),
								SoNguoiToiDa = reader.GetInt32("SoNguoiToiDa"),
								GiaNgay = reader.GetDecimal("GiaNgay"),
								GiaGio = reader.GetDecimal("GiaGio"),
								ImageId = reader["ImageId"].ToString(),
								ImageURL = reader["ImageURL"].ToString()
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return loaiPhongs;
		}

		public LoaiPhong getLoaiPhongTheoMaLoaiPhong(int maLoaiPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT * FROM LoaiPhong WHERE MaLoaiPhong = @MaLoaiPhong AND IsDeleted = 0";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", maLoaiPhong);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
							return new LoaiPhong
							{
								MaLoaiPhong = reader.GetInt32("MaLoaiPhong"),
								TenLoaiPhong = reader.GetString("TenLoaiPhong"),
								MoTa = reader["MoTa"].ToString(),
								ChinhSach = reader["ChinhSach"].ToString(),
								ChinhSachHuy = reader["ChinhSachHuy"].ToString(),
								SoNguoiToiDa = reader.GetInt32("SoNguoiToiDa"),
								GiaNgay = reader.GetDecimal("GiaNgay"),
								GiaGio = reader.GetDecimal("GiaGio"),
								ImageId = reader["ImageId"].ToString(),
								ImageURL = reader["ImageURL"].ToString()
							};
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return null;
		}

		// Thêm loại phòng mới
		public bool addLoaiPhong(LoaiPhong loaiPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			int maLoaiPhong = 0;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					conn.Open(); // ✅ Mở kết nối trước

					var query = @"
				INSERT INTO LoaiPhong 
					(TenLoaiPhong, MoTa, ChinhSach, ChinhSachHuy, SoNguoiToiDa, GiaNgay, GiaGio, ImageId, ImageURL, IsDeleted)
				VALUES 
					(@TenLoaiPhong, @MoTa, @ChinhSach, @ChinhSachHuy, @SoNguoiToiDa, @GiaNgay, @GiaGio, @ImageId, @ImageURL, 0);
				SELECT LAST_INSERT_ID();";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiPhong", loaiPhong.TenLoaiPhong);
					cmd.Parameters.AddWithValue("@MoTa", loaiPhong.MoTa);
					cmd.Parameters.AddWithValue("@ChinhSach", loaiPhong.ChinhSach);
					cmd.Parameters.AddWithValue("@ChinhSachHuy", loaiPhong.ChinhSachHuy);
					cmd.Parameters.AddWithValue("@SoNguoiToiDa", loaiPhong.SoNguoiToiDa);
					cmd.Parameters.AddWithValue("@GiaNgay", loaiPhong.GiaNgay);
					cmd.Parameters.AddWithValue("@GiaGio", loaiPhong.GiaGio);
					cmd.Parameters.AddWithValue("@ImageId", loaiPhong.ImageId ?? "hotel_management/image_default");
					cmd.Parameters.AddWithValue("@ImageURL", loaiPhong.ImageURL ?? "https://res.cloudinary.com/dzaoyffio/image/upload/v1750100245/image_default.png");

					object result = cmd.ExecuteScalar();
					if (result == null || result == DBNull.Value)
						throw new Exception("Không thể lấy mã loại phòng.");

					maLoaiPhong = Convert.ToInt32(result);
				}

				loaiPhong.MaLoaiPhong = maLoaiPhong;
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi thêm loại phòng: " + ex.Message);
				return false;
			}
		}


		// Xóa loại phòng
		public bool xoaLoaiPhong(LoaiPhong loaiPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE LoaiPhong SET IsDeleted = 1 WHERE MaLoaiPhong = @MaLoaiPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", loaiPhong.MaLoaiPhong);

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

		// Cập nhật loại phòng
		public bool capnhatLoaiPhong(LoaiPhong loaiPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
				UPDATE LoaiPhong 
				SET TenLoaiPhong = @TenLoaiPhong, MoTa = @MoTa, ChinhSach = @ChinhSach, ChinhSachHuy = @ChinhSachHuy,
					SoNguoiToiDa = @SoNguoiToiDa, GiaNgay = @GiaNgay, GiaGio = @GiaGio,
					ImageId = @ImageId, ImageURL = @ImageURL
				WHERE MaLoaiPhong = @MaLoaiPhong";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiPhong", loaiPhong.TenLoaiPhong);
					cmd.Parameters.AddWithValue("@MoTa", loaiPhong.MoTa);
					cmd.Parameters.AddWithValue("@ChinhSach", loaiPhong.ChinhSach);
					cmd.Parameters.AddWithValue("@ChinhSachHuy", loaiPhong.ChinhSachHuy);
					cmd.Parameters.AddWithValue("@SoNguoiToiDa", loaiPhong.SoNguoiToiDa);
					cmd.Parameters.AddWithValue("@GiaNgay", loaiPhong.GiaNgay);
					cmd.Parameters.AddWithValue("@GiaGio", loaiPhong.GiaGio);
					cmd.Parameters.AddWithValue("@ImageId", loaiPhong.ImageId);
					cmd.Parameters.AddWithValue("@ImageURL", loaiPhong.ImageURL);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", loaiPhong.MaLoaiPhong);

					conn.Open();
					cmd.ExecuteNonQuery();
				}

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		// Kiểm tra tên loại phòng có trùng không
		public bool KiemTraTenLoaiPhong(LoaiPhong loaiPhong)
		{
			var isExist = false;
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM LoaiPhong WHERE TenLoaiPhong = @TenLoaiPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiPhong", loaiPhong.TenLoaiPhong);

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

		// Hiển thị lại loại phòng đã xóa
		public bool hienThiLaiLoaiPhong(string tenLoaiPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE LoaiPhong SET IsDeleted = 0 WHERE TenLoaiPhong = @TenLoaiPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiPhong", tenLoaiPhong);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						Console.WriteLine($"Đã khôi phục loại phòng: {tenLoaiPhong}");
						return true;
					}

					Console.WriteLine($"Không tìm thấy loại phòng với tên: {tenLoaiPhong}");
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		public void capNhatHinhAnhLoaiPhong(int maLoaiPhong, string imageId, string imageUrl)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			using (var conn = new MySqlConnection(connectionString))
			{
				conn.Open();
				var query = @"UPDATE LoaiPhong 
							SET ImageId = @ImageId, ImageURL = @ImageURL 
							WHERE MaLoaiPhong = @MaLoaiPhong";
				var cmd = new MySqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@ImageId", imageId);
				cmd.Parameters.AddWithValue("@ImageURL", imageUrl);
				cmd.Parameters.AddWithValue("@MaLoaiPhong", maLoaiPhong);
				cmd.ExecuteNonQuery();
			}
		}
	}
}