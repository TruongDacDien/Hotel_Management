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
    public class HoaDonDAL
    {
		private static HoaDonDAL Instance;

		private HoaDonDAL() { }

		public static HoaDonDAL GetInstance()
		{
			if (Instance == null)
			{
				Instance = new HoaDonDAL();
			}
			return Instance;
		}

		// Lấy tất cả các hóa đơn
		public List<HoaDon> LayDuLieuHoaDon()
		{
			List<HoaDon> lstHoaDon = new List<HoaDon>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "SELECT MaHD, MaNV, MaCTPT, NgayLap, TongTien FROM HoaDon";
					SqlCommand cmd = new SqlCommand(query, conn);
					conn.Open();

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							lstHoaDon.Add(new HoaDon
							{
								MaHD = reader.GetInt32(reader.GetOrdinal("MaHD")),
								MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
								MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
								TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien"))
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}
			return lstHoaDon;
		}

		// Lấy thông tin hóa đơn theo mã hóa đơn
		public HoaDon layHoaDonTheoMaHoaDon(int mahd)
		{
			HoaDon hoaDon = null;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "SELECT MaHD, MaNV, MaCTPT, NgayLap, TongTien FROM HoaDon WHERE MaHD = @MaHD";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaHD", mahd);
					conn.Open();

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							hoaDon = new HoaDon
							{
								MaHD = reader.GetInt32(reader.GetOrdinal("MaHD")),
								MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
								MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
								TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien"))
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}
			return hoaDon;
		}

		// Thêm hóa đơn mới
		public bool themHoaDon(HoaDon hd, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = @"
                        INSERT INTO HoaDon (MaHD, MaPhieuThue, NgayLap, TongTien)
                        VALUES (@MaHD, @MaPhieuThue, @NgayLap, @TongTien)";
					SqlCommand cmd = new SqlCommand(query, conn);

					cmd.Parameters.AddWithValue("@MaHD", hd.MaHD);
					cmd.Parameters.AddWithValue("@MaPhieuThue", hd.MaCTPT);
					cmd.Parameters.AddWithValue("@NgayLap", hd.NgayLap);
					cmd.Parameters.AddWithValue("@TongTien", hd.TongTien);

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
	}
}
