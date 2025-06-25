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
	public class TaiKhoanNVDAL
	{
		private static TaiKhoanNVDAL Instance;

		private TaiKhoanNVDAL()
		{
		}

		public static TaiKhoanNVDAL GetInstance()
		{
			if (Instance == null) Instance = new TaiKhoanNVDAL();
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
									tknv.Email, tknv.LastLogin, tknv.MaXacNhan, tknv.ThoiGianHetHan, tknv.Disabled, tknv.IsDeleted,
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
								Disabled = reader.IsDBNull(reader.GetOrdinal("Disabled")) ? false : reader.GetBoolean(reader.GetOrdinal("Disabled")),
								IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
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
		public bool capNhatAvatar(int maTKNV, string avatarId, string avatarURL)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE TaiKhoanNV SET AvatarId = @AvatarId, AvatarURL = @AvatarURL WHERE MaTKNV = @MaTKNV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@AvatarId", avatarId);
					cmd.Parameters.AddWithValue("@AvatarURL", avatarURL);
					cmd.Parameters.AddWithValue("@MaTKNV", maTKNV);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected == 0)
					{
						return false;
					}
				}

				return true;
			}
			catch (Exception ex)
			{
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
									tknv.Email, tknv.LastLogin, tknv.MaXacNhan, tknv.ThoiGianHetHan, tknv.Disabled, tknv.IsDeleted,
									nv.HoTen, nv.ChucVu, nv.SDT, nv.DiaChi, nv.CCCD, nv.CCCDImage, nv.NTNS, nv.GioiTinh, nv.Luong, nv.IsDeleted
                                  FROM TaiKhoanNV tknv
                                  LEFT JOIN NhanVien nv ON tknv.MaNV = nv.MaNV 
								  WHERE tknv.IsDeleted = 0 AND nv.IsDeleted = 0";

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
								IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
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
					var query = @"INSERT INTO TaiKhoanNV (Username, Password, Email, AvatarId, AvatarURL, MaNV, LastLogin, Disabled, MaXacNhan, ThoiGianHetHan)
								VALUES (@Username, @Password, @Email, @AvatarId, @AvatarURL, @MaNV, NOW(), 0, 'xxx', NOW());
								SELECT LAST_INSERT_ID();";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", taiKhoan.Username);
					cmd.Parameters.AddWithValue("@Password", taiKhoan.Password);
					cmd.Parameters.AddWithValue("@Email", taiKhoan.Email);
					cmd.Parameters.AddWithValue("@AvatarId", taiKhoan.AvatarId ?? "hotel_management/user_default");
					cmd.Parameters.AddWithValue("@AvatarURL", taiKhoan.AvatarURL ?? "https://res.cloudinary.com/dzaoyffio/image/upload/v1747814352/hotel_management/user_default.png");
					cmd.Parameters.AddWithValue("@MaNV", taiKhoan.MaNV);
					conn.Open();
					object result = cmd.ExecuteScalar();
					if (result == null || result == DBNull.Value)
						throw new Exception("Không thể lấy mã dịch vụ sau khi thêm.");

					taiKhoan.MaTKNV = Convert.ToInt32(result);
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
					var query = @"UPDATE TaiKhoanNV 
								  SET Username = @Username, 
									MaNV = @MaNV, 
									Password = @Password, 
									Email = @Email, 
									AvatarId = @AvatarId, 
									AvatarURL = @AvatarURL,
									Disabled = @Disabled
								  WHERE MaTKNV = @MaTKNV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", taiKhoan.Username);
					cmd.Parameters.AddWithValue("@Password", taiKhoan.Password);
					cmd.Parameters.AddWithValue("@MaNV", taiKhoan.MaNV);
					cmd.Parameters.AddWithValue("@Email", taiKhoan.Email);
					cmd.Parameters.AddWithValue("@MaTKNV", taiKhoan.MaTKNV);
					cmd.Parameters.AddWithValue("@AvatarId", taiKhoan.AvatarId ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@AvatarURL", taiKhoan.AvatarURL ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@Disabled", taiKhoan.Disabled);
					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
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
					var query = "UPDATE TaiKhoan SET IsDeleted = 1 WHERE MaTKNV = @MaTKNV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTKNV", taiKhoan.MaTKNV);
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
								QLTaiKhoanNV = reader.IsDBNull(reader.GetOrdinal("QLTaiKhoanNV")) ? false : reader.GetBoolean(reader.GetOrdinal("QLTaiKhoanNV")),
								QLTaiKhoanKH = reader.IsDBNull(reader.GetOrdinal("QLTaiKhoanKH")) ? false : reader.GetBoolean(reader.GetOrdinal("QLTaiKhoanKH")),
								ThongKe = reader.IsDBNull(reader.GetOrdinal("ThongKe")) ? false : reader.GetBoolean(reader.GetOrdinal("ThongKe")),
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

		public bool capNhatPhanQuyenTaiKhoan(PhanQuyen pq)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                UPDATE PhanQuyen SET 
                    TrangChu = @TrangChu,
                    Phong = @Phong,
                    DatPhong = @DatPhong,
                    HoaDon = @HoaDon,
                    QLKhachHang = @QLKhachHang,
                    QLPhong = @QLPhong,
                    QLLoaiPhong = @QLLoaiPhong,
                    QLDichVu = @QLDichVu,
                    QLLoaiDichVu = @QLLoaiDichVu,
                    QLTienNghi = @QLTienNghi,
                    QLNhanVien = @QLNhanVien,
                    QLTaiKhoanNV = @QLTaiKhoanNV,
                    QLTaiKhoanKH = @QLTaiKhoanKH,
                    ThongKe = @ThongKe,
                    QLDatDV = @QLDatDV,
                    QLDDXQ = @QLDDXQ
                WHERE MaTKNV = @MaTKNV";

					var cmd = new MySqlCommand(query, conn);

					cmd.Parameters.AddWithValue("@TrangChu", pq.TrangChu);
					cmd.Parameters.AddWithValue("@Phong", pq.Phong);
					cmd.Parameters.AddWithValue("@DatPhong", pq.DatPhong);
					cmd.Parameters.AddWithValue("@HoaDon", pq.HoaDon);
					cmd.Parameters.AddWithValue("@QLKhachHang", pq.QLKhachHang);
					cmd.Parameters.AddWithValue("@QLPhong", pq.QLPhong);
					cmd.Parameters.AddWithValue("@QLLoaiPhong", pq.QLLoaiPhong);
					cmd.Parameters.AddWithValue("@QLDichVu", pq.QLDichVu);
					cmd.Parameters.AddWithValue("@QLLoaiDichVu", pq.QLLoaiDichVu);
					cmd.Parameters.AddWithValue("@QLTienNghi", pq.QLTienNghi);
					cmd.Parameters.AddWithValue("@QLNhanVien", pq.QLNhanVien);
					cmd.Parameters.AddWithValue("@QLTaiKhoanNV", pq.QLTaiKhoanNV);
					cmd.Parameters.AddWithValue("@QLTaiKhoanKH", pq.QLTaiKhoanKH);
					cmd.Parameters.AddWithValue("@ThongKe", pq.ThongKe);
					cmd.Parameters.AddWithValue("@QLDatDV", pq.QLDatDV);
					cmd.Parameters.AddWithValue("@QLDDXQ", pq.QLDDXQ);
					cmd.Parameters.AddWithValue("@MaTKNV", pq.MaTKNV);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi cập nhật phân quyền: {ex.Message}");
				return false;
			}
		}

		public bool themPhanQuyenTaiKhoan(PhanQuyen pq)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                INSERT INTO PhanQuyen (
                    MaTKNV, TrangChu, Phong, DatPhong, HoaDon, QLKhachHang, QLPhong, QLLoaiPhong,
                    QLDichVu, QLLoaiDichVu, QLTienNghi, QLNhanVien, QLTaiKhoanNV, QLTaiKhoanKH,
                    ThongKe, QLDatDV, QLDDXQ
                ) VALUES (
                    @MaTKNV, @TrangChu, @Phong, @DatPhong, @HoaDon, @QLKhachHang, @QLPhong, @QLLoaiPhong,
                    @QLDichVu, @QLLoaiDichVu, @QLTienNghi, @QLNhanVien, @QLTaiKhoanNV, @QLTaiKhoanKH,
                    @ThongKe, @QLDatDV, @QLDDXQ
                )";

					var cmd = new MySqlCommand(query, conn);

					cmd.Parameters.AddWithValue("@MaTKNV", pq.MaTKNV);
					cmd.Parameters.AddWithValue("@TrangChu", pq.TrangChu);
					cmd.Parameters.AddWithValue("@Phong", pq.Phong);
					cmd.Parameters.AddWithValue("@DatPhong", pq.DatPhong);
					cmd.Parameters.AddWithValue("@HoaDon", pq.HoaDon);
					cmd.Parameters.AddWithValue("@QLKhachHang", pq.QLKhachHang);
					cmd.Parameters.AddWithValue("@QLPhong", pq.QLPhong);
					cmd.Parameters.AddWithValue("@QLLoaiPhong", pq.QLLoaiPhong);
					cmd.Parameters.AddWithValue("@QLDichVu", pq.QLDichVu);
					cmd.Parameters.AddWithValue("@QLLoaiDichVu", pq.QLLoaiDichVu);
					cmd.Parameters.AddWithValue("@QLTienNghi", pq.QLTienNghi);
					cmd.Parameters.AddWithValue("@QLNhanVien", pq.QLNhanVien);
					cmd.Parameters.AddWithValue("@QLTaiKhoanNV", pq.QLTaiKhoanNV);
					cmd.Parameters.AddWithValue("@QLTaiKhoanKH", pq.QLTaiKhoanKH);
					cmd.Parameters.AddWithValue("@ThongKe", pq.ThongKe);
					cmd.Parameters.AddWithValue("@QLDatDV", pq.QLDatDV);
					cmd.Parameters.AddWithValue("@QLDDXQ", pq.QLDDXQ);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi thêm phân quyền: {ex.Message}");
				return false;
			}
		}

		public bool xoaPhanQuyenTaiKhoan(PhanQuyen pq)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "DELETE FROM PhanQuyen WHERE MaPQ = @MaPQ";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaPQ", pq.MaPQ);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();

					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Xử lý lỗi nếu cần
				return false;
			}
		}
	}
}