using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class CT_PhieuThueDAL
	{
		private static CT_PhieuThueDAL Instance;

		private CT_PhieuThueDAL()
		{
		}

		public static CT_PhieuThueDAL GetInstance()
		{
			if (Instance == null) Instance = new CT_PhieuThueDAL();
			return Instance;
		}

		// Thêm chi tiết phiếu thuê
		public bool addCTPhieuThue(CT_PhieuThue ctpt, out string error)
		{
			error = string.Empty;
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query =
						"INSERT INTO CT_PhieuThue (MaPhieuThue, SoPhong, NgayBD, NgayKT, SoNguoiO, TinhTrangThue, TienPhong) " +
						"VALUES (@MaPhieuThue, @SoPhong, @NgayBD, @NgayKT, @SoNguoiO, @TinhTrangThue, @TienPhong)";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhieuThue", ctpt.MaPhieuThue);
					cmd.Parameters.AddWithValue("@SoPhong", ctpt.SoPhong);
					cmd.Parameters.AddWithValue("@NgayBD", ctpt.NgayBD);
					cmd.Parameters.AddWithValue("@NgayKT", ctpt.NgayKT);
					cmd.Parameters.AddWithValue("@SoNguoiO", ctpt.SoNguoiO);
					cmd.Parameters.AddWithValue("@TinhTrangThue", ctpt.TinhTrangThue);
					cmd.Parameters.AddWithValue("@TienPhong", ctpt.TienPhong);

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

			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE CT_PhieuThue SET TinhTrangThue = @TinhTrangThue WHERE MaCTPT = @MaCTPT";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TinhTrangThue", tinhtrangthuephong);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
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
			var ls = new List<CT_PhieuThue>();
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT * FROM CT_PhieuThue WHERE MaPhieuThue = @MaPhieuThue";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhieuThue", maPT);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var ctpt = new CT_PhieuThue
							{
								MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
								SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
								NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
								SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
								TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
								TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong"))
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

		// Lấy chi tiết phiếu thuê theo mã chi tiết phiếu thuê
		public CT_PhieuThue getCT_PhieuThueTheoMaCTPT(int? maCTPT)
		{
			CT_PhieuThue cT_PhieuThue = null;
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT * FROM CT_PhieuThue WHERE MaCTPT = @MaCTPT";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							cT_PhieuThue = new CT_PhieuThue
							{
								MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
								SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
								NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
								NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
								SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
								TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
								TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong"))
							};
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return cT_PhieuThue;
		}

		// Cập nhật ngày bắt đầu
		public bool capNhatNgayBD(int? maCTPT, DateTime ngayBD, out string error)
		{
			error = string.Empty;
			if (maCTPT == null)
			{
				error = "Không tồn tại mã chi tiết phiếu thuê";
				return false;
			}

			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE CT_PhieuThue SET NgayBD = @NgayBD WHERE MaCTPT = @MaCTPT";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@NgayBD", ngayBD);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
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

		// Cập nhật ngày kết thúc
		public bool capNhatNgayKT(int? maCTPT, DateTime ngayKT, out string error)
		{
			error = string.Empty;
			if (maCTPT == null)
			{
				error = "Không tồn tại mã chi tiết phiếu thuê";
				return false;
			}

			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE CT_PhieuThue SET NgayKT = @NgayKT WHERE MaCTPT = @MaCTPT";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@NgayKT", ngayKT);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
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

		// Cập nhật tiền phòng
		public bool capNhatTien(int? maCTPT, decimal? tienPhong, out string errorCapNhatCTPT)
		{
			errorCapNhatCTPT = string.Empty;
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE CT_PhieuThue SET TienPhong = @TienPhong WHERE MaCTPT = @MaCTPT";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);
					cmd.Parameters.AddWithValue("@TienPhong", tienPhong);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

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