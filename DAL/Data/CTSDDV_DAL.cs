using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using DAL.DTO;

namespace DAL.Data
{
	public class CTSDDV_DAL
	{
		private static CTSDDV_DAL Instance;

		private CTSDDV_DAL() { }

		public static CTSDDV_DAL GetInstance()
		{
			if (Instance == null)
			{
				Instance = new CTSDDV_DAL();
			}
			return Instance;
		}

		// Thêm mới chi tiết sử dụng dịch vụ
		public bool addDataCTSDDC(CT_SDDichVu ctsddv, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"INSERT INTO CT_SDDichVu (MaCTPT, MaDV, SL, ThanhTien) 
                                     VALUES (@MaCTPT, @MaDV, @SL, @ThanhTien)";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", ctsddv.MaCTPT);
					cmd.Parameters.AddWithValue("@MaDV", ctsddv.MaDV);
					cmd.Parameters.AddWithValue("@SL", ctsddv.SL);
					cmd.Parameters.AddWithValue("@ThanhTien", ctsddv.ThanhTien);

					conn.Open();
					cmd.ExecuteNonQuery();
				}
				return true;
			}
			catch (Exception ex)
			{
				error = ex.Message;
				return false;
			}
		}

		// Tính tổng tiền chi tiết sử dụng dịch vụ theo mã CTPT
		public List<decimal> tongTienChiTietSuDungDichVu(int? maCTPT)
		{
			List<decimal> results = new List<decimal>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"SELECT ThanhTien FROM CT_SDDichVu WHERE MaCTPT = @MaCTPT";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							results.Add(reader.GetDecimal(reader.GetOrdinal("ThanhTien")));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}
			return results;
		}

		// Lấy danh sách sử dụng dịch vụ của 1 phòng dựa vào mã CTPT
		public List<DichVu_DaChon> getCTSDDVtheoMaCTPT(int? maCTPT)
		{
			List<DichVu_DaChon> ls = new List<DichVu_DaChon>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        SELECT 
                            ct.ThanhTien, 
                            ct.MaDV, 
                            dv.TenDV, 
                            ct.SL, 
                            dv.Gia
                        FROM CT_SDDichVu ct
                        INNER JOIN DichVu dv ON ct.MaDV = dv.MaDV
                        WHERE ct.MaCTPT = @MaCTPT";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							ls.Add(new DichVu_DaChon
							{
								ThanhTien = reader.GetDecimal(reader.GetOrdinal("ThanhTien")),
								MaDV = reader.GetInt32(reader.GetOrdinal("MaDV")),
								TenDV = reader.GetString(reader.GetOrdinal("TenDV")),
								SoLuong = reader.GetInt32(reader.GetOrdinal("SL")),
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
			return ls;
		}
	}
}
