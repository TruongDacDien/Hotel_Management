using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Documents;
using System.Windows;

namespace DAL.Data
{
	public class CT_TienNghiDAL
	{
		private static CT_TienNghiDAL instance;

		private CT_TienNghiDAL() { }

		public static CT_TienNghiDAL Instance
		{
			get
			{
				if (instance == null) instance = new CT_TienNghiDAL();
				return instance;
			}
		}

		// Lấy danh sách các chi tiết tiện nghi
		public List<CT_TienNghiDTO> getData()
		{
			List<CT_TienNghiDTO> listCTTienNghi = new List<CT_TienNghiDTO>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"SELECT CT.MaCTTN, CT.SoPhong, CT.SL, TN.TenTN 
									FROM CT_TienNghi CT
									JOIN TienNghi TN ON CT.MaTN = TN.MaTN";
					MySqlCommand cmd = new MySqlCommand(query, conn);
                    Console.WriteLine($"Query: {query}");
                    conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						if (!reader.HasRows)
						{
							Console.WriteLine("Không có dòng nào được trả về từ truy vấn.");
						}
					
                        while (reader.Read())
						{
                            Console.WriteLine($"Dòng dữ liệu: MaCTTN={reader["MaCTTN"]}, SoPhong={reader["SoPhong"]}, SL={reader["SL"]}, TenTN={reader["TenTN"]}");
                            CT_TienNghiDTO ctTienNghi = new CT_TienNghiDTO
							{
								MaCTTN = reader.GetInt32(reader.GetOrdinal("MaCTTN")),
								SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								SL = reader.GetInt32(reader.GetOrdinal("SL")),
								TenTN = reader.GetString(reader.GetOrdinal("TenTN"))
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
            MessageBox.Show($"Dữ liệu lấy được: {listCTTienNghi.Count}");
            return listCTTienNghi;
		}

		// Thêm một chi tiết tiện nghi mới
		public bool addCTTienNghi(CT_TienNghi chiTietTN)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "INSERT INTO CT_TienNghi (MaTN, SoPhong, SL) VALUES (@MaTN, @SoPhong, @SL)";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@SoPhong", chiTietTN.SoPhong);
					cmd.Parameters.AddWithValue("@SL", chiTietTN.SL);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "DELETE FROM CT_TienNghi WHERE MaCTTN = @MaCTTN";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTTN", chiTietTN.MaCTTN);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE CT_TienNghi SET MaTN = @MaTN, SoPhong = @SoPhong, SL = @SL WHERE MaCTTN = @MaCTTN";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTTN", chiTietTN.MaCTTN);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@SoPhong", chiTietTN.SoPhong);
					cmd.Parameters.AddWithValue("@SL", chiTietTN.SL);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Xử lý lỗi nếu cần
				return false;
			}
		}

		// Kiểm tra chi tiết tiện nghi có tồn tại hay không
		public bool KiemTraTonTai(CT_TienNghi chiTietTN)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT COUNT(1) FROM CT_TienNghi WHERE MaTN = @MaTN AND SoPhong = @SoPhong";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@SoPhong", chiTietTN.SoPhong);

					conn.Open();
					int count = Convert.ToInt32(cmd.ExecuteScalar());

					return count > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Xử lý lỗi nếu cần
				return false;
			}
		}
	}
}
