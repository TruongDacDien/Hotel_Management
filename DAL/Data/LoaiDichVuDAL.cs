using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class LoaiDichVuDAL
	{
		private static LoaiDichVuDAL instance;

		private LoaiDichVuDAL() { }

		public static LoaiDichVuDAL Instance
		{
			get
			{
				if (instance == null) instance = new LoaiDichVuDAL();
				return instance;
			}
		}

		// Lấy tất cả loại dịch vụ
		public List<LoaiDV> getData()
		{
			List<LoaiDV> loaiDichVus = new List<LoaiDV>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT * FROM LoaiDichVu";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							loaiDichVus.Add(new LoaiDV
							{
								MaLoaiDV = reader.GetInt32(reader.GetOrdinal("MaLoaiDV")),
								TenLoaiDV = reader.GetString(reader.GetOrdinal("TenLoaiDV"))
							});
						}
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        INSERT INTO LoaiDichVu (TenLoaiDV)
                        VALUES (@TenLoaiDV)";

					MySqlCommand cmd = new MySqlCommand(query, conn);
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "DELETE FROM LoaiDichVu WHERE MaLoaiDV = @MaLoaiDV";
					MySqlCommand cmd = new MySqlCommand(query, conn);
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        UPDATE LoaiDichVu 
                        SET TenLoaiDV = @TenLoaiDV
                        WHERE MaLoaiDV = @MaLoaiDV";

					MySqlCommand cmd = new MySqlCommand(query, conn);
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
			bool isExist = false;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT COUNT(*) FROM LoaiDichVu WHERE TenLoaiDV = @TenLoaiDV";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenLoaiDV", loaiDV.TenLoaiDV);

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
