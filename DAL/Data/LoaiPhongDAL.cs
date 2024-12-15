using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class LoaiPhongDAL
	{
		private static LoaiPhongDAL instance;

		private LoaiPhongDAL()
		{
		}

		public static LoaiPhongDAL Instance
		{
			get
			{
				if (instance == null) instance = new LoaiPhongDAL();
				return instance;
			}
		}

		// Lấy tất cả loại phòng
		public List<LoaiPhong> getDataLoaiPhong()
		{
			var loaiPhongs = new List<LoaiPhong>();
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

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
								MaLoaiPhong = reader.GetInt32(reader.GetOrdinal("MaLoaiPhong")),
								TenLoaiPhong = reader.GetString(reader.GetOrdinal("TenLoaiPhong")),
								SoNguoiToiDa = reader.GetInt32(reader.GetOrdinal("SoNguoiToiDa")),
								GiaNgay = reader.GetDecimal(reader.GetOrdinal("GiaNgay")),
								GiaGio = reader.GetDecimal(reader.GetOrdinal("GiaGio"))
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return loaiPhongs;
		}

		// Thêm loại phòng mới
		public bool addLoaiPhong(LoaiPhong loaiPhong)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        INSERT INTO LoaiPhong (TenLoaiPhong, SoNguoiToiDa, GiaNgay, GiaGio, IsDeleted)
                        VALUES (@TenLoaiPhong, @SoNguoiToiDa, @GiaNgay, @GiaGio, 0)";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiPhong", loaiPhong.TenLoaiPhong);
					cmd.Parameters.AddWithValue("@SoNguoiToiDa", loaiPhong.SoNguoiToiDa);
					cmd.Parameters.AddWithValue("@GiaNgay", loaiPhong.GiaNgay);
					cmd.Parameters.AddWithValue("@GiaGio", loaiPhong.GiaGio);

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

		// Xóa loại phòng
		public bool xoaLoaiPhong(LoaiPhong loaiPhong)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

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
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        UPDATE LoaiPhong 
                        SET TenLoaiPhong = @TenLoaiPhong, SoNguoiToiDa = @SoNguoiToiDa, GiaNgay = @GiaNgay, GiaGio = @GiaGio
                        WHERE MaLoaiPhong = @MaLoaiPhong";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiPhong", loaiPhong.TenLoaiPhong);
					cmd.Parameters.AddWithValue("@SoNguoiToiDa", loaiPhong.SoNguoiToiDa);
					cmd.Parameters.AddWithValue("@GiaNgay", loaiPhong.GiaNgay);
					cmd.Parameters.AddWithValue("@GiaGio", loaiPhong.GiaGio);
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

		// Kiểm tra tên loại phòng có trùng không
		public bool KiemTraTenLoaiPhong(LoaiPhong loaiPhong)
		{
			var isExist = false;
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

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
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

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
	}
}