using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Hoặc bất kỳ định dạng hình ảnh nào bạn cần
				return ms.ToArray(); // Trả về mảng byte từ MemoryStream
			}
		}

		/*
		// Chuyển đổi mảng byte thành hình ảnh
		public Image ConvertByteArrayToImage(byte[] byteArray)
		{
			using (MemoryStream ms = new MemoryStream(byteArray))
			{
				return Image.FromStream(ms); // Chuyển mảng byte thành Image
			}
		}
		*/

		public TaiKhoan layTaiKhoanTuDataBase(string username, string pass)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					// Truy vấn kết hợp để lấy thông tin tài khoản và nhân viên
					string query = @"
                SELECT 
                    tk.username, tk.password, tk.maNV, tk.capDoQuyen, tk.avatar,
                    nv.hoTen, nv.chucVu, nv.sDT, nv.diaChi, nv.cCCD, nv.nTNS, nv.gioiTinh, nv.luong, nv.maTK
                FROM TaiKhoan tk
                LEFT JOIN NhanVien nv ON tk.maNV = nv.maNV
                WHERE tk.username = @Username AND tk.password = @Password";

					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Username", username);
					cmd.Parameters.AddWithValue("@Password", pass);

					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							// Lấy avatar dưới dạng mảng byte (nếu có)
							byte[] avatarBytes = reader["avatar"] as byte[];

							// Trả về đối tượng TaiKhoan với thông tin NhanVien
							return new TaiKhoan
							{
								Username = reader.GetString(reader.GetOrdinal("username")),
								Password = reader.GetString(reader.GetOrdinal("password")),
								MaNV = reader.GetInt32(reader.GetOrdinal("maNV")),
								CapDoQuyen = reader.GetInt32(reader.GetOrdinal("capDoQuyen")),
								Avatar = avatarBytes,
								// Khởi tạo thông tin NhanVien
								NhanVien = new NhanVien
								{
									MaNV = reader.GetInt32(reader.GetOrdinal("maNV")),
									HoTen = reader.GetString(reader.GetOrdinal("hoTen")),
									ChucVu = reader.GetString(reader.GetOrdinal("chucVu")),
									SDT = reader.GetString(reader.GetOrdinal("sDT")),
									DiaChi = reader.GetString(reader.GetOrdinal("diaChi")),
									CCCD = reader.GetString(reader.GetOrdinal("cCCD")),
									NTNS = reader.GetDateTime(reader.GetOrdinal("nTNS")),
									GioiTinh = reader.GetString(reader.GetOrdinal("gioiTinh")),
									Luong = reader.GetDecimal(reader.GetOrdinal("luong")),
								}
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message); // Log lỗi nếu cần
			}

			return null; // Nếu không tìm thấy tài khoản
		}


		// Cập nhật avatar của tài khoản
		public bool capNhatAvatar(string username, byte[] avatarBytes, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "UPDATE TaiKhoan SET avatar = @Avatar WHERE username = @Username";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@Avatar", avatarBytes); // Truyền mảng byte của avatar vào câu lệnh
					cmd.Parameters.AddWithValue("@Username", username);

					conn.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected == 0)
					{
						error = "Không tồn tại tài khoản " + username;
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
