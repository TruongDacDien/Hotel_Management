using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class DAO_KhachHang
	{
		private static DAO_KhachHang instance;
		private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

		private DAO_KhachHang() { }

		public static DAO_KhachHang GetInstance()
		{
			if (instance == null)
			{
				instance = new DAO_KhachHang();
			}
			return instance;
		}

		// Thêm khách hàng
		public bool ThemKhachHang(KhachHang kh)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "INSERT INTO KhachHang (TENKH, GIOITINH, CCCD, SDT, EMAIL, DIACHI, QUOCTICH, MATK) VALUES (@TENKH, @GIOITINH, @CCCD, @SDT, @EMAIL, @DIACHI, @QUOCTICH, @MATK)";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TENKH", kh.TENKH);
					cmd.Parameters.AddWithValue("@GIOITINH", kh.GIOITINH);
					cmd.Parameters.AddWithValue("@CCCD", kh.CCCD);
					cmd.Parameters.AddWithValue("@SDT", kh.SDT);
					cmd.Parameters.AddWithValue("@EMAIL", kh.EMAIL);
					cmd.Parameters.AddWithValue("@DIACHI", kh.DIACHI);
					cmd.Parameters.AddWithValue("@QUOCTICH", kh.QUOCTICH);
					cmd.Parameters.AddWithValue("@MATK", kh.MATK);
					cmd.ExecuteNonQuery();
					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
					return false;
				}
			}
		}

		// Xóa khách hàng
		public bool XoaKhachHang(int maKH)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "DELETE FROM KhachHang WHERE MAKH = @MAKH";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MAKH", maKH);
					cmd.ExecuteNonQuery();
					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
					return false;
				}
			}
		}

		// Sửa thông tin khách hàng
		public bool SuaKhachHang(KhachHang kh)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "UPDATE KhachHang SET TENKH = @TENKH, GIOITINH = @GIOITINH, CCCD = @CCCD, SDT = @SDT, EMAIL = @EMAIL, DIACHI = @DIACHI, QUOCTICH = @QUOCTICH, MATK = @MATK WHERE MAKH = @MAKH";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MAKH", kh.MAKH);
					cmd.Parameters.AddWithValue("@TENKH", kh.TENKH);
					cmd.Parameters.AddWithValue("@GIOITINH", kh.GIOITINH);
					cmd.Parameters.AddWithValue("@CCCD", kh.CCCD);
					cmd.Parameters.AddWithValue("@SDT", kh.SDT);
					cmd.Parameters.AddWithValue("@EMAIL", kh.EMAIL);
					cmd.Parameters.AddWithValue("@DIACHI", kh.DIACHI);
					cmd.Parameters.AddWithValue("@QUOCTICH", kh.QUOCTICH);
					cmd.Parameters.AddWithValue("@MATK", kh.MATK);
					cmd.ExecuteNonQuery();
					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
					return false;
				}
			}
		}

		// Lấy tất cả khách hàng
		public List<KhachHang> ListKhachHang()
		{
			List<KhachHang> danhSachKhachHang = new List<KhachHang>();

			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "SELECT * FROM KhachHang";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							KhachHang kh = new KhachHang
							{
								MAKH = reader.GetInt32("MAKH"),
								TENKH = reader.GetString("TENKH"),
								GIOITINH = reader.GetString("GIOITINH"),
								CCCD = reader.GetString("CCCD"),
								SDT = reader.GetString("SDT"),
								EMAIL = reader.GetString("EMAIL"),
								DIACHI = reader.GetString("DIACHI"),
								QUOCTICH = reader.GetString("QUOCTICH"),
								MATK = reader.GetInt32("MATK")
							};
							danhSachKhachHang.Add(kh);
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
			}

			return danhSachKhachHang;
		}

		// Kiểm tra khách hàng tồn tại qua CCCD và trả về thông tin khách hàng
		public KhachHang KiemTraTonTaiQuaCCCD(string cccd)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				try
				{
					conn.Open();
					string query = "SELECT * FROM KhachHang WHERE CCCD = @CCCD";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@CCCD", cccd);

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							KhachHang kh = new KhachHang
							{
								MAKH = reader.GetInt32("MAKH"),
								TENKH = reader.GetString("TENKH"),
								GIOITINH = reader.GetString("GIOITINH"),
								CCCD = reader.GetString("CCCD"),
								SDT = reader.GetString("SDT"),
								EMAIL = reader.GetString("EMAIL"),
								DIACHI = reader.GetString("DIACHI"),
								QUOCTICH = reader.GetString("QUOCTICH"),
								MATK = reader.GetInt32("MATK")
							};
							return kh;
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
			}
			return null; // Trả về null nếu không tìm thấy khách hàng
		}
	}
}
