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

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string query = "SELECT * FROM TienNghi";
				SqlCommand cmd = new SqlCommand(query, conn);
				conn.Open();

				using (SqlDataReader reader = cmd.ExecuteReader())
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
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "INSERT INTO TienNghi (TenTN) VALUES (@TenTN)";
					SqlCommand cmd = new SqlCommand(query, conn);
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
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "DELETE FROM TienNghi WHERE MaTN = @MaTN";
					SqlCommand cmd = new SqlCommand(query, conn);
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
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "UPDATE TienNghi SET TenTN = @TenTN WHERE MaTN = @MaTN";
					SqlCommand cmd = new SqlCommand(query, conn);
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
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "SELECT COUNT(*) FROM TienNghi WHERE TenTN = @TenTN";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenTN", tn.TenTN);

					conn.Open();
					int count = (int)cmd.ExecuteScalar();
					return count > 0;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
