using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "SELECT * FROM CT_TienNghi";
					SqlCommand cmd = new SqlCommand(query, conn);
					conn.Open();

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							CT_TienNghiDTO ctTienNghi = new CT_TienNghiDTO
							{
								MaCTTN = reader.GetInt32(reader.GetOrdinal("MaCTTN")),
								MaTN = reader.GetInt32(reader.GetOrdinal("MaTN")),
								SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								SL = reader.GetInt32(reader.GetOrdinal("SL"))
							};
							listCTTienNghi.Add(ctTienNghi);
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có
				Console.WriteLine(ex.Message);
			}

			return listCTTienNghi;
		}

		// Thêm một chi tiết tiện nghi mới
		public bool addCTTienNghi(CT_TienNghi chiTietTN)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "INSERT INTO CT_TienNghi (MaTN, SoPhong, SL) VALUES (@MaTN, @SoPhong, @SL)";
					SqlCommand cmd = new SqlCommand(query, conn);
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
				// Xử lý lỗi nếu có
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		// Xóa chi tiết tiện nghi
		public bool xoaCTTienNghi(CT_TienNghi chiTietTN)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "DELETE FROM CT_TienNghi WHERE MaCTTN = @MaCTTN";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTTN", chiTietTN.MaCTTN);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		// Cập nhật chi tiết tiện nghi
		public bool capnhatCTTienNghi(CT_TienNghi chiTietTN)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "UPDATE CT_TienNghi SET MaTN = @MaTN, SoPhong = @SoPhong, SL = @SL WHERE MaCTTN = @MaCTTN";
					SqlCommand cmd = new SqlCommand(query, conn);
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
				// Xử lý lỗi nếu có
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		// Kiểm tra chi tiết tiện nghi có tồn tại hay không
		public bool KiemTraTonTai(CT_TienNghi chiTietTN)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "SELECT COUNT(1) FROM CT_TienNghi WHERE MaTN = @MaTN AND SoPhong = @SoPhong";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@SoPhong", chiTietTN.SoPhong);

					conn.Open();
					int count = (int)cmd.ExecuteScalar();

					return count > 0;
				}
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}
