using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using DAL.DTO;

namespace DAL.Data
{
	public class DichVuDAL
	{
		private static DichVuDAL Instance;

		private DichVuDAL() { }

		public static DichVuDAL GetInstance()
		{
			if (Instance == null)
			{
				Instance = new DichVuDAL();
			}
			return Instance;
		}

		// Lấy danh sách dịch vụ (Custom)
		public List<DichVu_Custom> getDataDichVu_Custom()
		{
			List<DichVu_Custom> lsNDVCT = new List<DichVu_Custom>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        SELECT 
                            dv.MaDV, dv.TenDV, dv.Gia, ldv.TenLoaiDV AS LoaiDV
                        FROM DichVu dv
                        INNER JOIN LoaiDV ldv ON dv.MaLoaiDV = ldv.MaLoaiDV";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							lsNDVCT.Add(new DichVu_Custom
							{
								MaDV = reader.GetInt32(reader.GetOrdinal("MaDV")),
								TenDV = reader.GetString(reader.GetOrdinal("TenDV")),
								Gia = reader.GetDecimal(reader.GetOrdinal("Gia")),
								LoaiDV = reader.GetString(reader.GetOrdinal("LoaiDV"))
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}
			return lsNDVCT;
		}

		// Lấy danh sách loại dịch vụ
		public List<string> getDataLoaiDichVu()
		{
			List<string> lsLoaiDV = new List<string>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT TenLoaiDV FROM LoaiDV";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							lsLoaiDV.Add(reader.GetString(reader.GetOrdinal("TenLoaiDV")));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}
			return lsLoaiDV;
		}

		// Lấy danh sách tất cả dịch vụ
		public List<DichVuDTO> getDataDichVu()
		{
			List<DichVuDTO> lsDichVu = new List<DichVuDTO>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT MaDV, TenDV, MaLoaiDV, Gia FROM DichVu";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							lsDichVu.Add(new DichVuDTO
							{
								MaDV = reader.GetInt32(reader.GetOrdinal("MaDV")),
								TenDV = reader.GetString(reader.GetOrdinal("TenDV")),
								MaLoaiDV = reader.GetInt32(reader.GetOrdinal("MaLoaiDV")),
								Gia = reader.GetDecimal(reader.GetOrdinal("Gia"))
							});
						}
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "INSERT INTO DichVu (TenDV, MaLoaiDV, Gia) VALUES (@TenDV, @MaLoaiDV, @Gia)";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
					cmd.Parameters.AddWithValue("@MaLoaiDV", dv.MaLoaiDV);
					cmd.Parameters.AddWithValue("@Gia", dv.Gia);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Cập nhật dịch vụ
		public bool capNhatDichVu(DichVu dv)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE DichVu SET TenDV = @TenDV, MaLoaiDV = @MaLoaiDV, Gia = @Gia WHERE MaDV = @MaDV";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaDV", dv.MaDV);
					cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);
					cmd.Parameters.AddWithValue("@MaLoaiDV", dv.MaLoaiDV);
					cmd.Parameters.AddWithValue("@Gia", dv.Gia);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Xóa dịch vụ
		public bool xoaDichVu(DichVu dv)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "DELETE FROM DichVu WHERE MaDV = @MaDV";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaDV", dv.MaDV);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT COUNT(*) FROM DichVu WHERE TenDV = @TenDV";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenDV", dv.TenDV);

					conn.Open();
					int count = Convert.ToInt32(cmd.ExecuteScalar());
					return count > 0; // Trả về true nếu tên dịch vụ đã tồn tại
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
