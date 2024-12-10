using DAL.DTO;
using System;
using System.Configuration;
using System.IO;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Collections.Generic;

namespace DAL.Data
{
	public class TaiKhoanDAL
	{
		private static TaiKhoanDAL Instance;

		private TaiKhoanDAL() { }

		public static TaiKhoanDAL GetInstance()
		{
			if (Instance == null)
			{
				Instance = new TaiKhoanDAL();
			}
			return Instance;
		}

		// Chuyển đổi hình ảnh thành mảng byte
		public byte[] ConvertImageToByteArray(Image image)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Hoặc định dạng khác
				return ms.ToArray();
			}
		}

		// Lấy tài khoản theo username
		public TaiKhoan layTaiKhoanTheoUsername(string username)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"SELECT tk.username, tk.password, tk.maNV, tk.capDoQuyen, tk.avatar, tk.disabled,
									nv.hoTen, nv.chucVu, nv.sDT, nv.diaChi, nv.cCCD, nv.nTNS, nv.gioiTinh, nv.luong
									FROM TaiKhoan tk
									LEFT JOIN NhanVien nv ON tk.maNV = nv.maNV
									WHERE tk.username = @Username";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							return new TaiKhoan
							{
								Username = reader["username"].ToString(),
								Password = reader["password"].ToString(),
								MaNV = reader.IsDBNull(reader.GetOrdinal("maNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("maNV")),
								CapDoQuyen = reader.IsDBNull(reader.GetOrdinal("capDoQuyen")) ? 0 : reader.GetInt32(reader.GetOrdinal("capDoQuyen")),
								Avatar = reader["avatar"] as byte[],
								Disabled = reader.IsDBNull(reader.GetOrdinal("disabled")) ? false : reader.GetBoolean(reader.GetOrdinal("disabled")),
								NhanVien = new NhanVien
								{
									MaNV = reader.IsDBNull(reader.GetOrdinal("maNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("maNV")),
									HoTen = reader["hoTen"]?.ToString(),
									ChucVu = reader["chucVu"]?.ToString(),
									SDT = reader["sDT"]?.ToString(),
									DiaChi = reader["diaChi"]?.ToString(),
									CCCD = reader["cCCD"]?.ToString(),
									NTNS = reader.IsDBNull(reader.GetOrdinal("nTNS")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("nTNS")),
									GioiTinh = reader["gioiTinh"]?.ToString(),
									Luong = reader.IsDBNull(reader.GetOrdinal("luong")) ? 0 : reader.GetDecimal(reader.GetOrdinal("luong"))
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

			return null; // Trả về null nếu không tìm thấy
		}

		// Cập nhật avatar của tài khoản
		public bool capNhatAvatar(string username, byte[] avatarBytes, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE TaiKhoan SET avatar = @Avatar WHERE username = @Username";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Avatar", avatarBytes);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

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
			List<TaiKhoan> danhSachTaiKhoan = new List<TaiKhoan>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"SELECT tk.Username, tk.Password, tk.MaNV, tk.CapDoQuyen, tk.Avatar, tk.Disabled,
                            nv.HoTen, nv.ChucVu, nv.SDT, nv.DiaChi, nv.CCCD, nv.NTNS, nv.GioiTinh, nv.Luong
                            FROM TaiKhoan tk
                            LEFT JOIN NhanVien nv ON tk.MaNV = nv.MaNV AND tk.Disabled = 0";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							TaiKhoan taiKhoan = new TaiKhoan
							{
								Username = reader["Username"].ToString(),
								Password = reader["Password"].ToString(),
								MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaNV")),
								CapDoQuyen = reader.IsDBNull(reader.GetOrdinal("CapDoQuyen")) ? 0 : reader.GetInt32(reader.GetOrdinal("CapDoQuyen")),
								Avatar = reader["avatar"] as byte[],
								Disabled = reader.IsDBNull(reader.GetOrdinal("Disabled")) ? false : reader.GetBoolean(reader.GetOrdinal("Disabled")),
								NhanVien = new NhanVien
								{
									MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("maNV")),
									HoTen = reader["HoTen"]?.ToString(),
									ChucVu = reader["ChucVu"]?.ToString(),
									SDT = reader["SDT"]?.ToString(),
									DiaChi = reader["DiaChi"]?.ToString(),
									CCCD = reader["CCCD"]?.ToString(),
									NTNS = reader.IsDBNull(reader.GetOrdinal("NTNS")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("NTNS")),
									GioiTinh = reader["GioiTinh"]?.ToString(),
									Luong = reader.IsDBNull(reader.GetOrdinal("Luong")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Luong"))
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"INSERT INTO TaiKhoan (Username, Password, MaNV, CapDoQuyen, Avatar, Disabled)
                             VALUES (@Username, @Password, @MaNV, @CapDoQuyen, @Avatar, @Disabled)";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", taiKhoan.Username);
					cmd.Parameters.AddWithValue("@Password", taiKhoan.Password);
					cmd.Parameters.AddWithValue("@MaNV", taiKhoan.MaNV);
					cmd.Parameters.AddWithValue("@CapDoQuyen", taiKhoan.CapDoQuyen);
					cmd.Parameters.AddWithValue("@Avatar", taiKhoan.Avatar ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@Disabled", taiKhoan.Disabled);

					conn.Open();
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public bool capNhatTaiKhoan(TaiKhoan taiKhoan)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"UPDATE TaiKhoan
                             SET Password = @Password,
                                 MaNV = @MaNV,
                                 CapDoQuyen = @CapDoQuyen,
                                 Avatar = @Avatar,
                                 Disabled = @Disabled
                             WHERE username = @Username";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", taiKhoan.Username);
					cmd.Parameters.AddWithValue("@Password", taiKhoan.Password);
					cmd.Parameters.AddWithValue("@MaNV", taiKhoan.MaNV);
					cmd.Parameters.AddWithValue("@CapDoQuyen", taiKhoan.CapDoQuyen);
					cmd.Parameters.AddWithValue("@Avatar", taiKhoan.Avatar ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@Disabled", taiKhoan.Disabled);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected == 0)
					{
						return false;
					}
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public bool kiemTraTrungUsername(string username)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT COUNT(*) FROM TaiKhoan WHERE Username = @Username";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					int count = Convert.ToInt32(cmd.ExecuteScalar());
					return count > 0; // Trả về true nếu username đã tồn tại
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE TaiKhoan SET Disabled = 1 WHERE Username = @Username";
					MySqlCommand cmd = new MySqlCommand(query, conn);
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
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE TaiKhoan SET Disabled = 0 WHERE Username = @Username";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						return true;
					}
					else
					{
						return false;
					}
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
