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
	public class TaiKhoanDAL
	{
		private static TaiKhoanDAL Instance;

		private TaiKhoanDAL()
		{
		}

		public static TaiKhoanDAL GetInstance()
		{
			if (Instance == null) Instance = new TaiKhoanDAL();
			return Instance;
		}

		// Chuyển đổi hình ảnh thành mảng byte
		public byte[] ConvertImageToByteArray(Image image)
		{
			using (var ms = new MemoryStream())
			{
				image.Save(ms, ImageFormat.Jpeg); // Hoặc định dạng khác
				return ms.ToArray();
			}
		}

		private byte[] GetDefaultAvatar()
		{
			var basePath = AppDomain.CurrentDomain.BaseDirectory; // Lấy thư mục gốc của ứng dụng
			var filepath = Path.Combine(basePath, "Res", "default_image.jpg");
			if (!File.Exists(filepath))
				throw new FileNotFoundException("Không tìm thấy file ảnh mặc định", filepath);
			using (var image = Image.FromFile(filepath))
			{
				return ConvertImageToByteArray(image);
			}
		}

		// Lấy tài khoản theo username
		public TaiKhoan layTaiKhoanTheoUsername(string username)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT tk.Username, tk.Password, tk.MaNV, tk.CapDoQuyen, tk.Avatar, tk.Email, tk.Disabled,
									nv.HoTen, nv.ChucVu, nv.SDT, nv.DiaChi, nv.CCCD, nv.NTNS, nv.GioiTinh, nv.Luong
									FROM TaiKhoan tk
									LEFT JOIN NhanVien nv ON tk.MaNV = nv.MaNV
									WHERE tk.Username = @Username";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
							return new TaiKhoan
							{
								Username = reader["Username"].ToString(),
								Password = reader["Password"].ToString(),
								MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV"))
									? 0
									: reader.GetInt32(reader.GetOrdinal("MaNV")),
								CapDoQuyen = reader.IsDBNull(reader.GetOrdinal("CapDoQuyen"))
									? 0
									: reader.GetInt32(reader.GetOrdinal("CapDoQuyen")),
								Avatar = reader["Avatar"] as byte[],
								Email = reader["Email"].ToString(),
								Disabled = reader.IsDBNull(reader.GetOrdinal("Disabled"))
									? false
									: reader.GetBoolean(reader.GetOrdinal("Disabled")),
								NhanVien = new NhanVien
								{
									MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV"))
										? 0
										: reader.GetInt32(reader.GetOrdinal("MaNV")),
									HoTen = reader["HoTen"]?.ToString(),
									ChucVu = reader["ChucVu"]?.ToString(),
									SDT = reader["SDT"]?.ToString(),
									DiaChi = reader["DiaChi"]?.ToString(),
									CCCD = reader["CCCD"]?.ToString(),
									NTNS = reader.IsDBNull(reader.GetOrdinal("NTNS"))
										? DateTime.MinValue
										: reader.GetDateTime(reader.GetOrdinal("NTNS")),
									GioiTinh = reader["GioiTinh"]?.ToString(),
									Luong = reader.IsDBNull(reader.GetOrdinal("Luong"))
										? 0
										: reader.GetDecimal(reader.GetOrdinal("Luong"))
								}
							};
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
			}

			return null; // Trả về null nếu không tìm thấy
		}

		// Cập nhật avatar của tài khoản
		public bool capNhatAvatar(string username, byte[] avatarBytes, out string error)
		{
			error = string.Empty;
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TaiKhoan SET avatar = @Avatar WHERE username = @Username";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Avatar", avatarBytes);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected == 0)
					{
						error = $"Không tìm thấy tài khoản {username}.";
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

		// Lấy danh sách tất cả tài khoản
		public List<TaiKhoan> getDataTaiKhoan()
		{
			var danhSachTaiKhoan = new List<TaiKhoan>();
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT tk.Username, tk.Password, tk.MaNV, tk.CapDoQuyen, tk.Avatar, tk.Email, tk.Disabled,
                            nv.HoTen, nv.ChucVu, nv.SDT, nv.DiaChi, nv.CCCD, nv.NTNS, nv.GioiTinh, nv.Luong
                            FROM TaiKhoan tk
                            LEFT JOIN NhanVien nv ON tk.MaNV = nv.MaNV 
							WHERE tk.Disabled = 0";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var taiKhoan = new TaiKhoan
							{
								Username = reader["Username"].ToString(),
								Password = reader["Password"].ToString(),
								MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV"))
									? 0
									: reader.GetInt32(reader.GetOrdinal("MaNV")),
								CapDoQuyen = reader.IsDBNull(reader.GetOrdinal("CapDoQuyen"))
									? 0
									: reader.GetInt32(reader.GetOrdinal("CapDoQuyen")),
								Avatar = reader["avatar"] as byte[],
								Email = reader["Email"].ToString(),
								Disabled = reader.IsDBNull(reader.GetOrdinal("Disabled"))
									? false
									: reader.GetBoolean(reader.GetOrdinal("Disabled")),
								NhanVien = new NhanVien
								{
									MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV"))
										? 0
										: reader.GetInt32(reader.GetOrdinal("maNV")),
									HoTen = reader["HoTen"]?.ToString(),
									ChucVu = reader["ChucVu"]?.ToString(),
									SDT = reader["SDT"]?.ToString(),
									DiaChi = reader["DiaChi"]?.ToString(),
									CCCD = reader["CCCD"]?.ToString(),
									NTNS = reader.IsDBNull(reader.GetOrdinal("NTNS"))
										? DateTime.MinValue
										: reader.GetDateTime(reader.GetOrdinal("NTNS")),
									GioiTinh = reader["GioiTinh"]?.ToString(),
									Luong = reader.IsDBNull(reader.GetOrdinal("Luong"))
										? 0
										: reader.GetDecimal(reader.GetOrdinal("Luong"))
								}
							};

							danhSachTaiKhoan.Add(taiKhoan);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
			}

			return danhSachTaiKhoan;
		}

		public bool themTaiKhoan(TaiKhoan taiKhoan)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"INSERT INTO TaiKhoan (Username, Password, MaNV, CapDoQuyen, Avatar, Email, Disabled)
                             VALUES (@Username, @Password, @MaNV, @CapDoQuyen, @Avatar, @Email,0)";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", taiKhoan.Username);
					cmd.Parameters.AddWithValue("@Password", taiKhoan.Password);
					cmd.Parameters.AddWithValue("@MaNV", taiKhoan.MaNV);
					cmd.Parameters.AddWithValue("@CapDoQuyen", taiKhoan.CapDoQuyen);
					cmd.Parameters.AddWithValue("@Avatar", taiKhoan.Avatar ?? GetDefaultAvatar());
					cmd.Parameters.AddWithValue("@Email", taiKhoan.Email);
					conn.Open();
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi: {ex.Message}");
				return false;
			}
		}

		public bool capNhatTaiKhoan(TaiKhoan taiKhoan)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				Console.WriteLine(taiKhoan.Username + " " + taiKhoan.Password + " " + taiKhoan.CapDoQuyen + " " +
				                  taiKhoan.MaNV);
				using (var conn = new MySqlConnection(connectionString))
				{
					var query =
						@"UPDATE TaiKhoan SET MaNV = @MaNV, CapDoQuyen = @CapDoQuyen, Password = @Password , Email = @Email WHERE Username = @Username";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", taiKhoan.Username);
					cmd.Parameters.AddWithValue("@Password", taiKhoan.Password);
					cmd.Parameters.AddWithValue("@MaNV", taiKhoan.MaNV);
					cmd.Parameters.AddWithValue("@CapDoQuyen", taiKhoan.CapDoQuyen);
					cmd.Parameters.AddWithValue("@Email", taiKhoan.Email);
					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					Console.WriteLine(rowsAffected);
					if (rowsAffected == 0) return false;
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public bool kiemTraTrungUsername(string username)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM TaiKhoan WHERE Username = @Username";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					var count = Convert.ToInt32(cmd.ExecuteScalar());
					return count > 0; // Trả về true nếu username đã tồn tại
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
				return false;
			}
		}

		public bool kiemTraTrungEmail(string email)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM TaiKhoan WHERE Email = @Email";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Email", email);

					conn.Open();
					var count = Convert.ToInt32(cmd.ExecuteScalar());
					return count > 0; // Trả về true nếu email đã tồn tại
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
				return false;
			}
		}

		public bool xoaTaiKhoan(TaiKhoan taiKhoan)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TaiKhoan SET Disabled = 1 WHERE Username = @Username";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", taiKhoan.Username);
					conn.Open();
					cmd.ExecuteNonQuery();
				}

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log error if needed
				return false;
			}
		}

		public bool hienThiLaiTaiKhoan(string username)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TaiKhoan SET Disabled = 0 WHERE Username = @Username";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0) return true;

					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
				return false;
			}
		}
	}
}