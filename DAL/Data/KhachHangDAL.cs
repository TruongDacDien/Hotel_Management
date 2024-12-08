using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class KhachHangDAL
	{
		private static KhachHangDAL Instance;

		private KhachHangDAL() { }

		public static KhachHangDAL GetInstance()
		{
			if (Instance == null)
			{
				Instance = new KhachHangDAL();
			}
			return Instance;
		}

		// Thêm khách hàng mới
		public bool addKhachHang(KhachHang kh, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        INSERT INTO KhachHang (TenKH, GioiTinh, CCCD, DiaChi, SDT, QuocTich, IsDeleted)
                        VALUES (@TenKH, @GioiTinh, @CCCD, @DiaChi, @SDT, @QuocTich, 0)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenKH", kh.TenKH);
                    cmd.Parameters.AddWithValue("@GioiTinh", kh.GioiTinh);
                    cmd.Parameters.AddWithValue("@CCCD", kh.CCCD);
					cmd.Parameters.AddWithValue("@DiaChi", kh.DiaChi);
					cmd.Parameters.AddWithValue("@SDT", kh.SDT); 
					cmd.Parameters.AddWithValue("@QuocTich", kh.QuocTich);

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

		// Kiểm tra tồn tại khách hàng dựa trên CCCD
		public KhachHang kiemTraTonTaiKhachHang(string CCCD)
		{
			KhachHang khachHang = null;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT * FROM KhachHang WHERE CCCD = @CCCD";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@CCCD", CCCD);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							khachHang = new KhachHang
							{

								MaKH = reader.GetInt32(reader.GetOrdinal("MaKH")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
								CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
								DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
								SDT = reader.GetString(reader.GetOrdinal("SDT")),
								QuocTich = reader.GetString(reader.GetOrdinal("QuocTich")),
								IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}
			return khachHang;
		}

		// Lấy tất cả khách hàng
		public List<KhachHang> getData()
		{
			List<KhachHang> lstKhachHang = new List<KhachHang>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT * FROM KhachHang WHERE IsDeleted = 0";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							lstKhachHang.Add(new KhachHang
							{
								MaKH = reader.GetInt32(reader.GetOrdinal("MaKH")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
								CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
								DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
								SDT = reader.GetString(reader.GetOrdinal("SDT")),
								QuocTich = reader.GetString(reader.GetOrdinal("QuocTich"))
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}
			return lstKhachHang;
		}

		// Cập nhật thông tin khách hàng
		public bool capnhatKhachHang(KhachHang khachHang)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			string error = string.Empty;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        UPDATE KhachHang 
                        SET TenKH = @TenKH, CCCD = @CCCD, GioiTinh = @GioiTinh, DiaChi = @DiaChi, SDT = @SDT, QuocTich = @QuocTich
                        WHERE MaKH = @MaKH";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TenKH", khachHang.TenKH);
					cmd.Parameters.AddWithValue("@CCCD", khachHang.CCCD);
                    cmd.Parameters.AddWithValue("@GioiTinh", khachHang.GioiTinh);
                    cmd.Parameters.AddWithValue("@DiaChi", khachHang.DiaChi);
					cmd.Parameters.AddWithValue("@SDT", khachHang.SDT);
					cmd.Parameters.AddWithValue("@QuocTich", khachHang.QuocTich);
					cmd.Parameters.AddWithValue("@MaKH", khachHang.MaKH);

					conn.Open();
					cmd.ExecuteNonQuery();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Xóa khách hàng
		public bool xoaKhachHang(KhachHang khachHang)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE KhachHang SET IsDeleted = 1 WHERE MaKH = @MaKH";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaKH", khachHang.MaKH);

					conn.Open();
					cmd.ExecuteNonQuery();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}

		// Lấy tên khách hàng theo mã phiếu thuê
		public string layTenKhachHangTheoMaPT(int? maPhieuThue)
		{
			string tenKhachHang = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT TenKH FROM KhachHang WHERE MaKH = (SELECT MaKH FROM PhieuThue WHERE MaPhieuThue = @MaPhieuThue)";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							tenKhachHang = reader.GetString(reader.GetOrdinal("TenKH"));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}
			return tenKhachHang;
		}

		public int layMaKHMoiNhat()
		{
			int maKH = -1; // Giá trị mặc định nếu không tìm thấy
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT MAX(MaKH) AS MaKH FROM KhachHang";
					MySqlCommand cmd = new MySqlCommand(query, conn);

					conn.Open();
					object result = cmd.ExecuteScalar();

					if (result != null && result != DBNull.Value)
					{
						maKH = Convert.ToInt32(result);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi lấy MaKH mới nhất: " + ex.Message);
			}

			return maKH;
		}

	}
}
