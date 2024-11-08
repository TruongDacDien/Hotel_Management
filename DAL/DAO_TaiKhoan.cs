using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
	public class DAO_TaiKhoan
	{
		private static DAO_TaiKhoan instance;
		private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

		private DAO_TaiKhoan() { }

		public static DAO_TaiKhoan GetInstance()
		{
			if (instance == null)
			{
				instance = new DAO_TaiKhoan();
			}
			return instance;
		}

		// Thêm tài khoản
		public bool ThemTaiKhoan(TaiKhoan tk)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "INSERT INTO TaiKhoan (TENTK, MK, EMAIL, MAPQ) VALUES (@TENTK, @MK, @EMAIL, @MAPQ)";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TENTK", tk.TENTK);
					cmd.Parameters.AddWithValue("@MK", tk.MK);
					cmd.Parameters.AddWithValue("@EMAIL", tk.EMAIL);
					cmd.Parameters.AddWithValue("@MAPQ", tk.MAPQ);

					int result = cmd.ExecuteNonQuery();
					return result > 0;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Lỗi khi thêm tài khoản: " + ex.Message);
					return false;
				}
			}
		}

		// Xóa tài khoản
		public bool XoaTaiKhoan(int maTK)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "DELETE FROM TaiKhoan WHERE MATK = @MATK";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MATK", maTK);

					int result = cmd.ExecuteNonQuery();
					return result > 0;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Lỗi khi xóa tài khoản: " + ex.Message);
					return false;
				}
			}
		}

		// Sửa tài khoản
		public bool SuaTaiKhoan(TaiKhoan tk)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "UPDATE TaiKhoan SET TENTK = @TENTK, MK = @MK, EMAIL = @EMAIL, MAPQ = @MAPQ WHERE MATK = @MATK";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TENTK", tk.TENTK);
					cmd.Parameters.AddWithValue("@MK", tk.MK);
					cmd.Parameters.AddWithValue("@EMAIL", tk.EMAIL);
					cmd.Parameters.AddWithValue("@MAPQ", tk.MAPQ);
					cmd.Parameters.AddWithValue("@MATK", tk.MATK);

					int result = cmd.ExecuteNonQuery();
					return result > 0;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Lỗi khi sửa tài khoản: " + ex.Message);
					return false;
				}
			}
		}

		// Lấy tất cả tài khoản
		public List<TaiKhoan> LayTatCaTaiKhoan()
		{
			List<TaiKhoan> danhSachTaiKhoan = new List<TaiKhoan>();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "SELECT MATK, TENTK, MK, EMAIL, MAPQ FROM TaiKhoan";
					SqlCommand cmd = new SqlCommand(query, conn);
					SqlDataReader reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						TaiKhoan tk = new TaiKhoan
						{
							MATK = reader.GetInt32(0),
							TENTK = reader.GetString(1),
							MK = reader.GetString(2),
							EMAIL = reader.GetString(3),
							MAPQ = reader.GetInt32(4)
						};
						danhSachTaiKhoan.Add(tk);
					}
					reader.Close();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Lỗi khi lấy danh sách tài khoản: " + ex.Message);
				}
			}

			return danhSachTaiKhoan;
		}

		// Lấy tài khoản theo mã
		public TaiKhoan LayTaiKhoanTheoMa(int maTK)
		{
			TaiKhoan tk = null;

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "SELECT MATK, TENTK, MK, EMAIL, MAPQ FROM TaiKhoan WHERE MATK = @MATK";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MATK", maTK);
					SqlDataReader reader = cmd.ExecuteReader();

					if (reader.Read())
					{
						tk = new TaiKhoan
						{
							MATK = reader.GetInt32(0),
							TENTK = reader.GetString(1),
							MK = reader.GetString(2),
							EMAIL = reader.GetString(3),
							MAPQ = reader.GetInt32(4)
						};
					}
					reader.Close();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Lỗi khi lấy tài khoản theo mã: " + ex.Message);
				}
			}

			return tk;
		}
	}
}
