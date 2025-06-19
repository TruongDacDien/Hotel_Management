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

		// Lấy tài khoản theo username
		public TaiKhoanNV layTaiKhoanTheoUsername(string username)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT tknv.MaTKNV, tknv.Username, tknv.Password, tknv.MaNV, tknv.AvatarId, tknv.AvatarURL,
									tknv.Email, tknv.LastLogin, tknv.MaXacNhan, tknv.ThoiGianHetHan, tknv.Disabled,
									nv.HoTen, nv.ChucVu, nv.SDT, nv.DiaChi, nv.CCCD, nv.CCCDImage, nv.NTNS, nv.GioiTinh, nv.Luong, nv.IsDeleted
								  FROM TaiKhoanNV tknv
								  LEFT JOIN NhanVien nv ON tknv.MaNV = nv.MaNV
								  WHERE tknv.Username = @Username";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							return new TaiKhoanNV
							{
								MaTKNV = reader.IsDBNull(reader.GetOrdinal("MaTKNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaTKNV")),
								Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader["Username"].ToString(),
								Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader["Password"].ToString(),
								MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaNV")),
								AvatarId = reader.IsDBNull(reader.GetOrdinal("AvatarId")) ? null : reader["AvatarId"].ToString(),
								AvatarURL = reader.IsDBNull(reader.GetOrdinal("AvatarURL")) ? null : reader["AvatarURL"].ToString(),
								Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader["Email"].ToString(),
								LastLogin = reader.IsDBNull(reader.GetOrdinal("LastLogin")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LastLogin")),
								MaXacNhan = reader.IsDBNull(reader.GetOrdinal("MaXacNhan")) ? null : reader["MaXacNhan"].ToString(),
								ThoiGianHetHan = reader.IsDBNull(reader.GetOrdinal("ThoiGianHetHan")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ThoiGianHetHan")),
								Disabled = reader.IsDBNull(reader.GetOrdinal("Disabled"))? false : reader.GetBoolean(reader.GetOrdinal("Disabled")),
								NhanVien = new NhanVien
								{
									MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaNV")),
									HoTen = reader.IsDBNull(reader.GetOrdinal("HoTen")) ? null : reader["HoTen"].ToString(),
									ChucVu = reader.IsDBNull(reader.GetOrdinal("ChucVu")) ? null : reader["ChucVu"].ToString(),
									SDT = reader.IsDBNull(reader.GetOrdinal("SDT")) ? null : reader["SDT"].ToString(),
									DiaChi = reader.IsDBNull(reader.GetOrdinal("DiaChi")) ? null : reader["DiaChi"].ToString(),
									CCCD = reader.IsDBNull(reader.GetOrdinal("CCCD")) ? null : reader["CCCD"].ToString(),
									CCCDImage = reader.IsDBNull(reader.GetOrdinal("CCCDImage")) ? null : reader["CCCDImage"].ToString(),
									NTNS = reader.IsDBNull(reader.GetOrdinal("NTNS")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("NTNS")),
									GioiTinh = reader.IsDBNull(reader.GetOrdinal("GioiTinh")) ? null : reader["GioiTinh"].ToString(),
									Luong = reader.IsDBNull(reader.GetOrdinal("Luong")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Luong")),
									IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
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

		// Cập nhật avatar của tài khoản
		public bool capNhatAvatar(string username, string avatarId, string avatarURL, out string error)
		{
			error = string.Empty;
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TaiKhoanNV SET AvatarId = @AvatarId, AvatarURL = @AvatarURL WHERE username = @Username";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@AvatarId", avatarId);
					cmd.Parameters.AddWithValue("@AvatarURL", avatarURL);
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
		public List<TaiKhoanNV> getDataTaiKhoan()
		{
			var danhSachTaiKhoan = new List<TaiKhoanNV>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT tknv.MaTKNV, tknv.Username, tknv.Password, tknv.MaNV, tknv.AvatarId, tknv.AvatarURL,
									tknv.Email, tknv.LastLogin, tknv.MaXacNhan, tknv.ThoiGianHetHan, tknv.Disabled,
									nv.HoTen, nv.ChucVu, nv.SDT, nv.DiaChi, nv.CCCD, nv.CCCDImage, nv.NTNS, nv.GioiTinh, nv.Luong, nv.IsDeleted
                                  FROM TaiKhoanNV tknv
                                  LEFT JOIN NhanVien nv ON tknv.MaNV = nv.MaNV 
								  WHERE tknv.Disabled = 0 AND nv.IsDeleted = 0";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var taiKhoan = new TaiKhoanNV
							{
								MaTKNV = reader.IsDBNull(reader.GetOrdinal("MaTKNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaTKNV")),
								Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader["Username"].ToString(),
								Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader["Password"].ToString(),
								MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaNV")),
								AvatarId = reader.IsDBNull(reader.GetOrdinal("AvatarId")) ? null : reader["AvatarId"].ToString(),
								AvatarURL = reader.IsDBNull(reader.GetOrdinal("AvatarURL")) ? null : reader["AvatarURL"].ToString(),
								Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader["Email"].ToString(),
								LastLogin = reader.IsDBNull(reader.GetOrdinal("LastLogin")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LastLogin")),
								MaXacNhan = reader.IsDBNull(reader.GetOrdinal("MaXacNhan")) ? null : reader["MaXacNhan"].ToString(),
								ThoiGianHetHan = reader.IsDBNull(reader.GetOrdinal("ThoiGianHetHan")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ThoiGianHetHan")),
								Disabled = reader.IsDBNull(reader.GetOrdinal("Disabled")) ? false : reader.GetBoolean(reader.GetOrdinal("Disabled")),
								NhanVien = new NhanVien
								{
									MaNV = reader.IsDBNull(reader.GetOrdinal("MaNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaNV")),
									HoTen = reader.IsDBNull(reader.GetOrdinal("HoTen")) ? null : reader["HoTen"].ToString(),
									ChucVu = reader.IsDBNull(reader.GetOrdinal("ChucVu")) ? null : reader["ChucVu"].ToString(),
									SDT = reader.IsDBNull(reader.GetOrdinal("SDT")) ? null : reader["SDT"].ToString(),
									DiaChi = reader.IsDBNull(reader.GetOrdinal("DiaChi")) ? null : reader["DiaChi"].ToString(),
									CCCD = reader.IsDBNull(reader.GetOrdinal("CCCD")) ? null : reader["CCCD"].ToString(),
									CCCDImage = reader.IsDBNull(reader.GetOrdinal("CCCDImage")) ? null : reader["CCCDImage"].ToString(),
									NTNS = reader.IsDBNull(reader.GetOrdinal("NTNS")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("NTNS")),
									GioiTinh = reader.IsDBNull(reader.GetOrdinal("GioiTinh")) ? null : reader["GioiTinh"].ToString(),
									Luong = reader.IsDBNull(reader.GetOrdinal("Luong")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Luong")),
									IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
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

		public bool themTaiKhoan(TaiKhoanNV taiKhoan)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"INSERT INTO TaiKhoanNV (Username, Password, Email, AvatarId, AvatarURL, MaTKNV, LastLogin, Disabled)
                             VALUES (@Username, @Password, @Email, @AvatarId, @AvatarURL, @MaTKNV, NOW(),0)";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", taiKhoan.Username);
					cmd.Parameters.AddWithValue("@Password", taiKhoan.Password);
					cmd.Parameters.AddWithValue("@Email", taiKhoan.Email);
					cmd.Parameters.AddWithValue("@AvatarId", taiKhoan.AvatarId);
					cmd.Parameters.AddWithValue("@AvatarURL", taiKhoan.AvatarURL);
					cmd.Parameters.AddWithValue("@MaTKNV", taiKhoan.MaNV);
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

		public bool capNhatTaiKhoan(TaiKhoanNV taiKhoan)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query =
						@"UPDATE TaiKhoanNV SET MaNV = @MaNV, Password = @Password , Email = @Email WHERE Username = @Username";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", taiKhoan.Username);
					cmd.Parameters.AddWithValue("@Password", taiKhoan.Password);
					cmd.Parameters.AddWithValue("@MaNV", taiKhoan.MaNV);
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
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM TaiKhoanNV WHERE Username = @Username";
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
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(*) FROM TaiKhoanNV WHERE Email = @Email";
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

		public bool xoaTaiKhoan(TaiKhoanNV taiKhoan)
		{
			var connectionString = Properties.Resources.MySqlConnection;
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
			var connectionString = Properties.Resources.MySqlConnection;

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

		public PhanQuyen layPhanQuyenTaiKhoan(int maTKNV)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT * FROM PhanQuyen WHERE MaTKNV = @MaTKNV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTKNV", maTKNV);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							return new PhanQuyen
							{
								MaPQ = reader.IsDBNull(reader.GetOrdinal("MaPQ")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaPQ")),
								MaTKNV = reader.IsDBNull(reader.GetOrdinal("MaTKNV")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaTKNV")),
								TrangChu = reader.IsDBNull(reader.GetOrdinal("TrangChu")) ? false : reader.GetBoolean(reader.GetOrdinal("TrangChu")),
								Phong = reader.IsDBNull(reader.GetOrdinal("Phong")) ? false : reader.GetBoolean(reader.GetOrdinal("Phong")),
								DatPhong = reader.IsDBNull(reader.GetOrdinal("DatPhong")) ? false : reader.GetBoolean(reader.GetOrdinal("DatPhong")),
								HoaDon = reader.IsDBNull(reader.GetOrdinal("HoaDon")) ? false : reader.GetBoolean(reader.GetOrdinal("HoaDon")),
								QLKhachHang = reader.IsDBNull(reader.GetOrdinal("QLKhachHang")) ? false : reader.GetBoolean(reader.GetOrdinal("QLKhachHang")),
								QLPhong = reader.IsDBNull(reader.GetOrdinal("QLPhong")) ? false : reader.GetBoolean(reader.GetOrdinal("QLPhong")),
								QLLoaiPhong = reader.IsDBNull(reader.GetOrdinal("QLLoaiPhong")) ? false : reader.GetBoolean(reader.GetOrdinal("QLLoaiPhong")),
								QLDichVu = reader.IsDBNull(reader.GetOrdinal("QLDichVu")) ? false : reader.GetBoolean(reader.GetOrdinal("QLDichVu")),
								QLLoaiDichVu = reader.IsDBNull(reader.GetOrdinal("QLLoaiDichVu")) ? false : reader.GetBoolean(reader.GetOrdinal("QLLoaiDichVu")),
								QLTienNghi = reader.IsDBNull(reader.GetOrdinal("QLTienNghi")) ? false : reader.GetBoolean(reader.GetOrdinal("QLTienNghi")),
								QLNhanVien = reader.IsDBNull(reader.GetOrdinal("QLNhanVien")) ? false : reader.GetBoolean(reader.GetOrdinal("QLNhanVien")),
								QLTaiKhoan = reader.IsDBNull(reader.GetOrdinal("QLTaiKhoan")) ? false : reader.GetBoolean(reader.GetOrdinal("QLTaiKhoan")),
								ThongKe = reader.IsDBNull(reader.GetOrdinal("ThongKe")) ? false : reader.GetBoolean(reader.GetOrdinal("ThongKe")),
								ThongBao = reader.IsDBNull(reader.GetOrdinal("ThongBao")) ? false : reader.GetBoolean(reader.GetOrdinal("ThongBao")),
								LichSuHoatDong = reader.IsDBNull(reader.GetOrdinal("LichSuHoatDong")) ? false : reader.GetBoolean(reader.GetOrdinal("LichSuHoatDong")),
                                QLDatDV = reader.IsDBNull(reader.GetOrdinal("QLDatDV")) ? false : reader.GetBoolean(reader.GetOrdinal("QLDatDV")),
                                QLDDXQ = reader.IsDBNull(reader.GetOrdinal("QLDDXQ")) ? false : reader.GetBoolean(reader.GetOrdinal("QLDDXQ"))
                            };
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi: {ex.Message}");
			}

			return null; // Trả về null nếu không tìm thấy hoặc có lỗi
		}
	}
}