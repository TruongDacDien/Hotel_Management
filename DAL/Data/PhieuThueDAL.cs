using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;

namespace DAL.Data
{
    public class PhieuThueDAL
    {
		private static PhieuThueDAL Instance;

		private PhieuThueDAL() { }

		public static PhieuThueDAL GetInstance()
		{
			if (Instance == null)
			{
				Instance = new PhieuThueDAL();
			}
			return Instance;
		}

		// Thêm mới phiếu thuê
		public bool addPhieuThue(PhieuThue pt, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = @"
                        INSERT INTO PhieuThue (MaPhieuThue, NgayLapPhieu, MaKH, MaNV)
                        VALUES (@MaPhieuThue, @NgayLapPhieu, @MaKH, @MaNV)";

					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhieuThue", pt.MaPhieuThue);
					cmd.Parameters.AddWithValue("@NgayLapPhieu", pt.NgayLapPhieu);
					cmd.Parameters.AddWithValue("@MaKH", pt.MaKH);
					cmd.Parameters.AddWithValue("@MaNV", pt.MaNV);

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

		// Xóa phiếu thuê theo mã phiếu thuê
		public bool xoaPhieuThueTheoMaPhieuThue(int maPhieuThue, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "DELETE FROM PhieuThue WHERE MaPhieuThue = @MaPhieuThue";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected == 0)
					{
						error = "Không tồn tại phiếu thuê có mã " + maPhieuThue;
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

		// Lấy dữ liệu phiếu thuê từ DB
		public List<PhieuThue_Custom> getDataFromDB()
		{
			List<PhieuThue_Custom> ls = new List<PhieuThue_Custom>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = @"
                        SELECT pt.MaPhieuThue, pt.NgayLapPhieu, kh.TenKH, nv.HoTen
                        FROM PhieuThue pt
                        JOIN KhachHang kh ON pt.MaKH = kh.MaKH
                        JOIN NhanVien nv ON pt.MaNV = nv.MaNV";

					SqlCommand cmd = new SqlCommand(query, conn);
					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							ls.Add(new PhieuThue_Custom
							{
								MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
								NgayLapPhieu = reader.GetDateTime(reader.GetOrdinal("NgayLapPhieu")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								TenNV = reader.GetString(reader.GetOrdinal("HoTen"))
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
