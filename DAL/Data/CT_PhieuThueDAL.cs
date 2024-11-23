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
    public class CT_PhieuThueDAL
    {
        private static CT_PhieuThueDAL Instance;

        private CT_PhieuThueDAL()
        {

        }

        public static CT_PhieuThueDAL GetInstance()
        {
            if (Instance == null)
            {
                Instance = new CT_PhieuThueDAL();
            }
            return Instance;
        }

		public bool addCTPhieuThue(CT_PhieuThue ctpt, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "INSERT INTO CT_PhieuThue (MaCTPT, MaPhieuThue, TinhTrangThue) " +
								   "VALUES (@MaCTPT, @MaPhieuThue, @TinhTrangThue)";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", ctpt.MaCTPT);
					cmd.Parameters.AddWithValue("@MaPhieuThue", ctpt.MaPhieuThue);
					cmd.Parameters.AddWithValue("@TinhTrangThue", ctpt.TinhTrangThue);

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
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "UPDATE CT_PhieuThue SET TinhTrangThue = @TinhTrangThue WHERE MaCTPT = @MaCTPT";
					SqlCommand cmd = new SqlCommand(query, conn);
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


		public List<CT_PhieuThue> getPhieuThueTheoMaPT(int maPT)
		{
			List<CT_PhieuThue> ls = new List<CT_PhieuThue>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "SELECT * FROM CT_PhieuThue WHERE MaPhieuThue = @MaPhieuThue";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhieuThue", maPT);

					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							CT_PhieuThue ctpt = new CT_PhieuThue
							{
								MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
								TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue"))
							};
							ls.Add(ctpt);
						}
					}9
				}
			}
			catch (Exception ex)
			{
				// Ghi log hoặc xử lý lỗi nếu cần
				Console.WriteLine(ex.Message);
			}
			return ls;
		}

		// Cập nhật số tiền phòng và ngày trả thực tế
		public bool capNhatTienVaNgayTraThucTe(int? maCTPT, decimal? tienPhong, DateTime now, out string errorCapNhatCTPT)
		{
			errorCapNhatCTPT = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					// Câu truy vấn cập nhật tiền phòng và ngày trả thực tế
					string query = "UPDATE CT_PhieuThue " +
								   "SET TienPhong = @TienPhong, NgayTraThucTe = @NgayTraThucTe " +
								   "WHERE MaCTPT = @MaCTPT";

					SqlCommand cmd = new SqlCommand(query, conn);
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
				// Xử lý lỗi và ghi thông báo lỗi
				errorCapNhatCTPT = ex.Message;
				return false;
			}
		}
	}
}
