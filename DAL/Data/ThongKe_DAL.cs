using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class ThongKe_DAL
	{
		private static ThongKe_DAL Instance;

		private ThongKe_DAL()
		{
		}

		public static ThongKe_DAL GetInstance()
		{
			if (Instance == null) Instance = new ThongKe_DAL();
			return Instance;
		}

		public ThongKe LoadThongKeTheoThangNam(int month, int year)
		{
			var kq = new ThongKe();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT 
									(SELECT SUM(CTPT.TienPhong)
									 FROM CT_PhieuThue CTPT
									 JOIN HoaDon HD ON CTPT.MaCTPT = HD.MaCTPT
									 WHERE MONTH(HD.NgayLap) = @month 
									   AND YEAR(HD.NgayLap) = @year
									   AND CTPT.TinhTrangThue = 'Đã thanh toán') AS TongTienPhong,
									SUM(CTDV.ThanhTien) AS TongTienDV,
									COUNT(DISTINCT HD.MaHD) AS SoLuongPhongDaDat,
									(SELECT SUM(CTPT.TienPhong)
									 FROM CT_PhieuThue CTPT
									 JOIN HoaDon HD ON CTPT.MaCTPT = HD.MaCTPT
									 WHERE MONTH(HD.NgayLap) = @month 
									   AND YEAR(HD.NgayLap) = @year
									   AND CTPT.TinhTrangThue = 'Đã thanh toán') + SUM(CTDV.ThanhTien) AS TongDoanhThu
								FROM HoaDon HD
								LEFT JOIN CT_PhieuThue CTPT ON CTPT.MaCTPT = HD.MaCTPT
								LEFT JOIN CT_SDDichVu CTDV ON CTDV.MaCTPT = CTPT.MaCTPT
								WHERE MONTH(HD.NgayLap) = @month 
								  AND YEAR(HD.NgayLap) = @year 
								  AND CTPT.TinhTrangThue = 'Đã thanh toán'";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@month", month);
					cmd.Parameters.AddWithValue("@year", year);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							kq.DoanhthuPhong = reader.IsDBNull(reader.GetOrdinal("TongTienPhong"))
								? 0
								: reader.GetDecimal(reader.GetOrdinal("TongTienPhong"));
							kq.DoanhthuDichVu = reader.IsDBNull(reader.GetOrdinal("TongTienDV"))
								? 0
								: reader.GetDecimal(reader.GetOrdinal("TongTienDV"));
							kq.Sophong = reader.IsDBNull(reader.GetOrdinal("SoLuongPhongDaDat"))
								? 0
								: reader.GetInt32(reader.GetOrdinal("SoLuongPhongDaDat"));
							kq.TongDoanhThu = reader.IsDBNull(reader.GetOrdinal("TongDoanhThu"))
								? 0
								: reader.GetDecimal(reader.GetOrdinal("TongDoanhThu"));
						}
					}
				}
				return kq;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public List<CTHD> LoadThongKeTheoNam(int year)
		{
			var kq = new List<CTHD>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT 
									CTPT.TienPhong,
									CTDV.ThanhTien AS TienDV,
									(IFNULL(CTPT.TienPhong, 0) + IFNULL(CTDV.ThanhTien, 0)) AS TongTien,
									HD.NgayLap
								FROM HoaDon HD
								LEFT JOIN CT_PhieuThue CTPT ON CTPT.MaCTPT = HD.MaCTPT
								LEFT JOIN CT_SDDichVu CTDV ON CTDV.MaCTPT = CTPT.MaCTPT
								WHERE YEAR(HD.NgayLap) = @year 
								  AND CTPT.TinhTrangThue = 'Đã thanh toán'";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@year", year);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							kq.Add(new CTHD
							{
								TienPhong = reader.IsDBNull(reader.GetOrdinal("TienPhong"))
									? 0
									: reader.GetDecimal(reader.GetOrdinal("TienPhong")),
								TienDV = reader.IsDBNull(reader.GetOrdinal("TienDV"))
									? 0
									: reader.GetDecimal(reader.GetOrdinal("TienDV")),
								TongTien = reader.IsDBNull(reader.GetOrdinal("TongTien"))
									? 0
									: reader.GetDecimal(reader.GetOrdinal("TongTien")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap"))
							});
						}
					}
				}

				return kq;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
	}
}