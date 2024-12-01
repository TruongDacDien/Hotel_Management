using DAL.DTO;
using System;
using System.Configuration;
using System.IO;
using MySql.Data.MySqlClient;
using System.Drawing;

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
					string query = @"
                    SELECT 
                        tk.username, tk.password, tk.maNV, tk.capDoQuyen, tk.avatar,
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
	}
}
