using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class CT_PhieuThueDAL
	{
		private static CT_PhieuThueDAL Instance;

		private CT_PhieuThueDAL() { }

		public static CT_PhieuThueDAL GetInstance()
		{
			if (Instance == null)
			{
				Instance = new CT_PhieuThueDAL();
			}
			return Instance;
		}

		// Thêm chi tiết phiếu thuê
		public bool addCTPhieuThue(CT_PhieuThue ctpt, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "INSERT INTO CT_PhieuThue (MaPhieuThue, SoPhong, NgayBD, NgayKT, SoNguoiO, TinhTrangThue, TienPhong, NgayTraThucTe) " +
								   "VALUES (@MaPhieuThue, @SoPhong, @NgayBD, @NgayKT, @SoNguoiO, @TinhTrangThue, @TienPhong, @NgayTraThucTe)";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhieuThue", ctpt.MaPhieuThue);
					cmd.Parameters.AddWithValue("@SoPhong", ctpt.SoPhong);
					cmd.Parameters.AddWithValue("@NgayBD", ctpt.NgayBD);
					cmd.Parameters.AddWithValue("@NgayKT", ctpt.NgayKT);
					cmd.Parameters.AddWithValue("@SoNguoiO", ctpt.SoNguoiO);
					cmd.Parameters.AddWithValue("@TinhTrangThue", ctpt.TinhTrangThue);
					cmd.Parameters.AddWithValue("@TienPhong", ctpt.TienPhong);
					cmd.Parameters.AddWithValue("@NgayTraThucTe", ctpt.NgayTraThucTe);

					conn.Open();
					cmd.ExecuteNonQuery();
				}
				return true;
			}
			catch (Exception e)
			{
				error = e.Message;
				return false;
			}
		}

		// Sửa trạng thái thuê phòng
		public bool suaTinhTrangThuePhong(int? maCTPT, string tinhtrangthuephong, out string error)
		{
			error = string.Empty;
			if (maCTPT == null)
			{
				error = "Không tồn tại mã chi tiết phiếu thuê";
				return false;
			}

			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE CT_PhieuThue SET TinhTrangThue = @TinhTrangThue WHERE MaCTPT = @MaCTPT";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TinhTrangThue", tinhtrangthuephong);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
					if (rowsAffected == 0)
					{
						error = "Không tồn tại chi tiết phiếu thuê có mã: " + maCTPT;
						return false;
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				error = ex.Message;
				return false;
			}
		}

		// Lấy danh sách phiếu thuê theo mã phiếu thuê
		public List<CT_PhieuThue> getPhieuThueTheoMaPT(int maPT)
		{
			List<CT_PhieuThue> ls = new List<CT_PhieuThue>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT * FROM CT_PhieuThue WHERE MaPhieuThue = @MaPhieuThue";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhieuThue", maPT);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							CT_PhieuThue ctpt = new CT_PhieuThue
							{
								MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
								SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
								NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
								SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
								TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
								TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong")),
								NgayTraThucTe = reader.GetDateTime(reader.GetOrdinal("NgayTraThucTe"))
							};
							ls.Add(ctpt);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return ls;
		}

		// Cập nhật tiền phòng và ngày trả thực tế
		public bool capNhatTienVaNgayTraThucTe(int? maCTPT, decimal? tienPhong, DateTime now, out string errorCapNhatCTPT)
		{
			errorCapNhatCTPT = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE CT_PhieuThue " +
								   "SET TienPhong = @TienPhong, NgayTraThucTe = @NgayTraThucTe " +
								   "WHERE MaCTPT = @MaCTPT";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);
					cmd.Parameters.AddWithValue("@TienPhong", tienPhong);
					cmd.Parameters.AddWithValue("@NgayTraThucTe", now);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected == 0)
					{
						errorCapNhatCTPT = "Không tìm thấy chi tiết phiếu thuê với mã CTPT " + maCTPT;
						return false;
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				errorCapNhatCTPT = ex.Message;
				return false;
			}
		}
	}
}
