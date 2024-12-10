using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows;

namespace DAL.Data
{
	public class TienNghiDAL
	{
		private static TienNghiDAL instance;

		private TienNghiDAL() { }

		public static TienNghiDAL Instance
		{
			get
			{
				if (instance == null) instance = new TienNghiDAL();
				return instance;
			}
		}

		// Lấy tất cả dữ liệu tiện nghi từ cơ sở dữ liệu
		public List<TienNghi> getData()
		{
			List<TienNghi> tienNghiList = new List<TienNghi>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				string query = "SELECT * FROM TienNghi WHERE IsDeleted = 0";
				MySqlCommand cmd = new MySqlCommand(query, conn);
				conn.Open();

				using (MySqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						TienNghi tn = new TienNghi
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "INSERT INTO TienNghi (TenTN, IsDeleted) VALUES (@TenTN, 0)";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tn.TenTN);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE TienNghi SET IsDeleted = 1 WHERE MaTN = @MaTN";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", tn.MaTN);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE TienNghi SET TenTN = @TenTN WHERE MaTN = @MaTN";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tn.TenTN);
					cmd.Parameters.AddWithValue("@MaTN", tn.MaTN);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT COUNT(*) FROM TienNghi WHERE TenTN = @TenTN";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tn.TenTN);

					conn.Open();
					int count = Convert.ToInt32(cmd.ExecuteScalar());

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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE TienNghi SET IsDeleted = 0 WHERE TenTN = @TenTN";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tenTN);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						Console.WriteLine($"Đã khôi phục tiện nghi: {tenTN}");
						return true;
					}
					else
					{
						Console.WriteLine($"Không tìm thấy loại phòng với tên: {tenTN}");
						return false;
					}
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
