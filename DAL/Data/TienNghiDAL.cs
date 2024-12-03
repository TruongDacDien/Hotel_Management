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
				string query = "SELECT * FROM TienNghi";
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
					string query = "INSERT INTO TienNghi (TenTN) VALUES (@TenTN)";
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
					string query = "DELETE FROM TienNghi WHERE MaTN = @MaTN";
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

   //     public List<CT_TienNghiDTO> getDataBySoPhong(string maPhong)
   //     {
   //         List<CT_TienNghiDTO> ctTienNghis = new List<CT_TienNghiDTO>();

   //         string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
   //         string query = "SELECT CT.SoPhong, CT.SL, CT.MaTN, TN.TenTN FROM CT_TienNghi CT JOIN TienNghi TN ON CT.MaTN = TN.MaTN WHERE CT.SoPhong = @SoPhong";

   //         try
   //         {
   //             using (MySqlConnection conn = new MySqlConnection(connectionString))
   //             {
   //                 MySqlCommand cmd = new MySqlCommand(query, conn);
   //                 cmd.Parameters.AddWithValue("@SoPhong", maPhong);
			//		Console.WriteLine(query);
   //                 conn.Open();
   //                 MySqlDataReader reader = cmd.ExecuteReader();

   //                 while (reader.Read())
   //                 {
   //                     CT_TienNghiDTO ctTienNghi = new CT_TienNghiDTO
   //                     {
   //                         SoPhong = reader.GetString("SoPhong"),
   //                         SL = reader.GetInt32("SL"),
   //                         MaTN = reader.GetInt32("MaTN"),
   //                         TenTN = reader.GetString("TenTN")
   //                     };
   //                     ctTienNghis.Add(ctTienNghi);
   //                 }
   //             }
   //         }
   //         catch (Exception ex)
   //         {
   //             Console.WriteLine("Error: " + ex.Message);
   //         }
			//MessageBox.Show(ctTienNghis.Count.ToString());
   //         return ctTienNghis;
   //     }
    }
}
