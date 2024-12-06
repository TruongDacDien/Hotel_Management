using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class LoaiPhongDAL
	{
		private static LoaiPhongDAL instance;

		private LoaiPhongDAL() { }

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
			List<LoaiPhong> loaiPhongs = new List<LoaiPhong>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT * FROM LoaiPhong WHERE IsDeleted = 0";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        INSERT INTO LoaiPhong (TenLoaiPhong, SoNguoiToiDa, GiaNgay, GiaGio, IsDeleted)
                        VALUES (@TenLoaiPhong, @SoNguoiToiDa, @GiaNgay, @GiaGio, 0)";

					MySqlCommand cmd = new MySqlCommand(query, conn);
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE LoaiPhong SET IsDeleted = 1 WHERE MaLoaiPhong = @MaLoaiPhong";
					MySqlCommand cmd = new MySqlCommand(query, conn);
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        UPDATE LoaiPhong 
                        SET TenLoaiPhong = @TenLoaiPhong, SoNguoiToiDa = @SoNguoiToiDa, GiaNgay = @GiaNgay, GiaGio = @GiaGio
                        WHERE MaLoaiPhong = @MaLoaiPhong";

					MySqlCommand cmd = new MySqlCommand(query, conn);
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
			bool isExist = false;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT COUNT(*) FROM LoaiPhong WHERE TenLoaiPhong = @TenLoaiPhong";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiPhong", loaiPhong.TenLoaiPhong);

					conn.Open();
					int count = Convert.ToInt32(cmd.ExecuteScalar());
					if (count > 0)
					{
						isExist = true;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}
			return isExist;
		}
	}
}
