using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Windows.Controls;

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
		public byte[] ConvertImageToByteArray(string imagePath)
		{
			return File.ReadAllBytes(imagePath); // Đọc tệp hình ảnh và trả về mảng byte
		}

		// Chuyển đổi mảng byte thành hình ảnh
		public Image ConvertByteArrayToImage(byte[] byteArray)
		{
			using (MemoryStream ms = new MemoryStream(byteArray))
			{
				return Image.FromStream(ms); // Chuyển mảng byte thành Image
			}
		}

		// Lấy tài khoản từ cơ sở dữ liệu theo username và password (đã mã hóa)
		public TaiKhoan layTaiKhoanTuDataBase(string username, string pass)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string query = "SELECT * FROM TaiKhoan WHERE username = @Username AND password = @Password";
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@Username", username);
				cmd.Parameters.AddWithValue("@Password", pass);

				conn.Open();
				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						byte[] avatarBytes = reader["avatar"] as byte[]; // Lấy dữ liệu avatar dưới dạng byte array
						Image avatarImage = null;

						if (avatarBytes != null)
						{
							avatarImage = ConvertByteArrayToImage(avatarBytes); // Chuyển byte array thành Image
						}

						return new TaiKhoan
						{
							MaTK = reader.GetInt32(reader.GetOrdinal("MaTK")),
							Username = reader.GetString(reader.GetOrdinal("username")),
							Password = reader.GetString(reader.GetOrdinal("password")),
							Email = reader.GetString(reader.GetOrdinal("email")),
							MaPQ = reader.GetInt32(reader.GetOrdinal("maPQ")),
							Avatar = avatarImage // Lưu đối tượng Image vào Avatar
						};
					}
				}
			}

			return null; // Nếu không tìm thấy tài khoản
		}

		// Cập nhật avatar của tài khoản
		public bool capNhatAvatar(string username, string avatarPath, out string error)
		{
			error = string.Empty;
			byte[] avatarBytes = ConvertImageToByteArray(avatarPath); // Chuyển đổi ảnh thành mảng byte

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
