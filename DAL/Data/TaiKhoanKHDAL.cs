using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class TaiKhoanKHDAL
	{
		private static TaiKhoanKHDAL Instance;
		private TaiKhoanKHDAL() { }

		public static TaiKhoanKHDAL GetInstance()
		{
			if (Instance == null) Instance = new TaiKhoanKHDAL();
			return Instance;
		}

		public TaiKhoanKH layTaiKhoanTheoUsername(string username)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT tk.MaTKKH, tk.Username, tk.Password, tk.Email, tk.AvatarId, tk.AvatarURL,
										   tk.MaKH, tk.LastLogin, tk.MaXacNhan, tk.ThoiGianHetHan, tk.Disabled,
										   kh.TenKH, kh.GioiTinh, kh.CCCD, kh.CCCDImage, kh.SDT, kh.DiaChi, kh.QuocTich, kh.IsDeleted
									FROM TaiKhoanKH tk
									LEFT JOIN KhachHang kh ON tk.MaKH = kh.MaKH
									WHERE tk.Username = @Username";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							return new TaiKhoanKH
							{
								MaTKKH = reader.GetInt32("MaTKKH"),
								Username = reader["Username"].ToString(),
								Password = reader["Password"].ToString(),
								Email = reader["Email"].ToString(),
								AvatarId = reader["AvatarId"]?.ToString(),
								AvatarURL = reader["AvatarURL"]?.ToString(),
								MaKH = reader.GetInt32("MaKH"),
								LastLogin = reader.GetDateTime("LastLogin"),
								MaXacNhan = reader["MaXacNhan"].ToString(),
								ThoiGianHetHan = reader.GetDateTime("ThoiGianHetHan"),
								Disabled = reader.GetBoolean("Disabled"),
								KhachHang = new KhachHang
								{
									MaKH = reader.GetInt32("MaKH"),
									TenKH = reader["TenKH"]?.ToString(),
									GioiTinh = reader["GioiTinh"]?.ToString(),
									CCCD = reader["CCCD"]?.ToString(),
									CCCDImage = reader["CCCDImage"]?.ToString(),
									SDT = reader["SDT"]?.ToString(),
									DiaChi = reader["DiaChi"]?.ToString(),
									QuocTich = reader["QuocTich"]?.ToString(),
									IsDeleted = reader.GetBoolean("IsDeleted")
								}
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
			}
			return null;
		}

		public List<TaiKhoanKH> getDataTaiKhoanKH()
		{
			var danhSachTaiKhoanKH = new List<TaiKhoanKH>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT tk.MaTKKH, tk.Username, tk.Password, tk.Email, tk.AvatarId, tk.AvatarURL,
                                tk.MaKH, tk.LastLogin, tk.MaXacNhan, tk.ThoiGianHetHan, tk.Disabled,
                                kh.TenKH, kh.GioiTinh, kh.CCCD, kh.CCCDImage, kh.SDT, kh.DiaChi, kh.QuocTich, kh.IsDeleted
                         FROM TaiKhoanKH tk
                         LEFT JOIN KhachHang kh ON tk.MaKH = kh.MaKH
                         WHERE kh.IsDeleted = 0 OR kh.IsDeleted IS NULL";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var taiKhoanKH = new TaiKhoanKH
							{
								MaTKKH = reader.GetInt32("MaTKKH"),
								Username = reader["Username"].ToString(),
								Password = reader["Password"].ToString(),
								Email = reader["Email"].ToString(),
								AvatarId = reader["AvatarId"]?.ToString(),
								AvatarURL = reader["AvatarURL"]?.ToString(),
								MaKH = reader.GetInt32("MaKH"),
								LastLogin = reader.GetDateTime("LastLogin"),
								MaXacNhan = reader["MaXacNhan"].ToString(),
								ThoiGianHetHan = reader.GetDateTime("ThoiGianHetHan"),
								Disabled = reader.GetBoolean("Disabled"),
								KhachHang = new KhachHang
								{
									MaKH = reader.GetInt32("MaKH"),
									TenKH = reader["TenKH"]?.ToString(),
									GioiTinh = reader["GioiTinh"]?.ToString(),
									CCCD = reader["CCCD"]?.ToString(),
									CCCDImage = reader["CCCDImage"]?.ToString(),
									SDT = reader["SDT"]?.ToString(),
									DiaChi = reader["DiaChi"]?.ToString(),
									QuocTich = reader["QuocTich"]?.ToString(),
									IsDeleted = reader.GetBoolean("IsDeleted")
								}
							};
							danhSachTaiKhoanKH.Add(taiKhoanKH);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi lấy danh sách tài khoản khách hàng: {ex.Message}\nStackTrace: {ex.StackTrace}");
			}

			return danhSachTaiKhoanKH;
		}

		// Thêm tài khoản khách hàng
		public bool themTaiKhoanKH(TaiKhoanKH kh)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"INSERT INTO TaiKhoanKH (Username, Password, Email, AvatarId, AvatarURL, MaKH, LastLogin, Disabled, MaXacNhan, ThoiGianHetHan)
                          VALUES (@Username, @Password, @Email, @AvatarId, @AvatarURL, @MaKH, NOW(), 0, 'xxx', NOW());
                          SELECT LAST_INSERT_ID();";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", kh.Username);
					cmd.Parameters.AddWithValue("@Password", kh.Password);
					cmd.Parameters.AddWithValue("@Email", kh.Email);
					cmd.Parameters.AddWithValue("@AvatarId", kh.AvatarId ?? "hotel_management/user_default");
					cmd.Parameters.AddWithValue("@AvatarURL", kh.AvatarURL ?? "https://res.cloudinary.com/dzaoyffio/image/upload/v1747814352/hotel_management/user_default.png");
					cmd.Parameters.AddWithValue("@MaKH", kh.MaKH);

					conn.Open();
					object result = cmd.ExecuteScalar();
					if (result == null || result == DBNull.Value)
						return false;

					kh.MaTKKH = Convert.ToInt32(result);
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
				return false;
			}
		}

		// Cập nhật tài khoản khách hàng
		public bool capNhatTaiKhoanKH(TaiKhoanKH kh)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"UPDATE TaiKhoanKH 
									SET Password = @Password,
										Email = @Email,
										AvatarId = @AvatarId,
										AvatarURL = @AvatarURL,
										Disabled = @Disabled
									WHERE MaTKKH = @MaTKKH";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Password", kh.Password);
					cmd.Parameters.AddWithValue("@Email", kh.Email);
					cmd.Parameters.AddWithValue("@AvatarId", kh.AvatarId ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@AvatarURL", kh.AvatarURL ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@Disabled", kh.Disabled);
					cmd.Parameters.AddWithValue("@MaTKKH", kh.MaTKKH);

					conn.Open();
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
				return false;
			}
		}

		// Cập nhật avatar
		public bool capNhatAvatarKH(int maTKKH, string avatarId, string avatarURL)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TaiKhoanKH SET AvatarId = @AvatarId, AvatarURL = @AvatarURL WHERE MaTKKH = @MaTKKH";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@AvatarId", avatarId);
					cmd.Parameters.AddWithValue("@AvatarURL", avatarURL);
					cmd.Parameters.AddWithValue("@MaTKKH", maTKKH);

					conn.Open();
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
				return false;
			}
		}

		// Kiểm tra trùng username
		public bool kiemTraTrungUsernameKH(string username)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM TaiKhoanKH WHERE Username = @Username";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
				return false;
			}
		}

		// Kiểm tra trùng email
		public bool kiemTraTrungEmailKH(string email)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM TaiKhoanKH WHERE Email = @Email";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Email", email);

					conn.Open();
					return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
				return false;
			}
		}

		// Vô hiệu hóa tài khoản
		public bool xoaTaiKhoanKH(TaiKhoanKH kh)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TaiKhoanKH SET Disabled = 1 WHERE MaTKKH = @MaTKKH";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTKKH", kh.MaTKKH);

					conn.Open();
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
				return false;
			}
		}

		// Kích hoạt lại tài khoản
		public bool hienThiLaiTaiKhoanKH(string username)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TaiKhoanKH SET Disabled = 0 WHERE Username = @Username";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
				return false;
			}
		}
	}
}
