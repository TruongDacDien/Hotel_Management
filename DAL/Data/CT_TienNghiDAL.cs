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
		public List<CT_TienNghi> getData()
		{
			List<CT_TienNghi> listCTTienNghi = new List<CT_TienNghi>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"SELECT CT.MaCTTN, CT.SoPhong, CT.SL, TN.TenTN, CT.MaTN
									FROM CT_TienNghi CT
									JOIN TienNghi TN ON CT.MaTN = TN.MaTN";
					MySqlCommand cmd = new MySqlCommand(query, conn);
                    conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
					
                        while (reader.Read())
						{
                          
                            CT_TienNghi ctTienNghi = new CT_TienNghi
							{
								MaCTTN = reader.GetInt32(reader.GetOrdinal("MaCTTN")),
								MaTN = reader.GetInt32(reader.GetOrdinal("MaTN")),
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
					string query = @"INSERT INTO CT_TienNghi (MaTN, SoPhong, SL, TenTN)
									SELECT @MaTN, @SoPhong, @SL, tn.TenTN
									FROM TienNghi tn
									WHERE tn.MaTN = @MaTN";
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
					string query = @"
							UPDATE CT_TienNghi
							SET 
								MaTN = @MaTN,
								SoPhong = @SoPhong,
								SL = @SL,
								TenTN = @TenTN
							WHERE MaCTTN = @MaCTTN;";
					using (MySqlCommand cmd = new MySqlCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@MaCTTN", chiTietTN.MaCTTN);
						cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
						cmd.Parameters.AddWithValue("@SoPhong", chiTietTN.SoPhong);
						cmd.Parameters.AddWithValue("@SL", chiTietTN.SL);
						cmd.Parameters.AddWithValue("@TenTN", chiTietTN.TenTN);

						conn.Open();

						int rowsAffected = cmd.ExecuteNonQuery();

						Console.WriteLine($"Rows affected: {rowsAffected}");

						return rowsAffected > 0;
					}
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
