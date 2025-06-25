using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class CT_TienNghiDAL
	{
		private static CT_TienNghiDAL instance;

		private CT_TienNghiDAL()
		{
		}

		public static CT_TienNghiDAL GetInstance()
		{
			if (instance == null) instance = new CT_TienNghiDAL();
			return instance;
		}

		// Lấy danh sách các chi tiết tiện nghi
		public List<CT_TienNghi> getData()
		{
			var listCTTienNghi = new List<CT_TienNghi>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
					SELECT CT.MaCTTN, CT.MaLoaiPhong, CT.SL, TN.TenTN, CT.MaTN
					FROM CT_TienNghi CT
					JOIN TienNghi TN ON CT.MaTN = TN.MaTN";
					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var ctTienNghi = new CT_TienNghi
							{
								MaCTTN = reader.GetInt32(reader.GetOrdinal("MaCTTN")),
								MaTN = reader.GetInt32(reader.GetOrdinal("MaTN")),
								MaLoaiPhong = reader.GetInt32(reader.GetOrdinal("MaLoaiPhong")),
								SL = reader.GetInt32(reader.GetOrdinal("SL")),
								TenTN = reader.GetString(reader.GetOrdinal("TenTN")),
							};
							listCTTienNghi.Add(ctTienNghi);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Xử lý lỗi nếu cần
			}

			return listCTTienNghi;
		}

		// Thêm một chi tiết tiện nghi mới
		public bool addCTTienNghi(CT_TienNghi chiTietTN)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
				INSERT INTO CT_TienNghi (MaTN, MaLoaiPhong, SL)
				VALUES (@MaTN, @MaLoaiPhong, @SL)";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", chiTietTN.MaLoaiPhong);
					cmd.Parameters.AddWithValue("@SL", chiTietTN.SL);

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

		// Xóa chi tiết tiện nghi
		public bool xoaCTTienNghi(CT_TienNghi chiTietTN)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "DELETE FROM CT_TienNghi WHERE MaCTTN = @MaCTTN";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTTN", chiTietTN.MaCTTN);

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

		// Cập nhật chi tiết tiện nghi
		public bool capnhatCTTienNghi(CT_TienNghi chiTietTN)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"UPDATE CT_TienNghi
									SET SL = @SL
									WHERE MaTN = @MaTN AND MaLoaiPhong = @MaLoaiPhong";
					var cmd = new MySqlCommand(query, conn);
					//cmd.Parameters.AddWithValue("@MaCTTN", chiTietTN.MaCTTN);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", chiTietTN.MaLoaiPhong);
					cmd.Parameters.AddWithValue("@SL", chiTietTN.SL);

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

		// Kiểm tra chi tiết tiện nghi có tồn tại hay không
		public bool KiemTraTonTai(CT_TienNghi chiTietTN)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT COUNT(1) FROM CT_TienNghi WHERE MaTN = @MaTN AND MaLoaiPhong = @MaLoaiPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", chiTietTN.MaTN);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", chiTietTN.MaLoaiPhong);

					conn.Open();
					var count = Convert.ToInt32(cmd.ExecuteScalar());

					return count > 0;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Xử lý lỗi nếu cần
				return false;
			}
		}

		// Hiển thị lại chi tiết tiện nghi đã xóa
		public bool hienThiLaiCT_TienNghi(int maTN, int maLoaiPhong)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE CT_TienNghi SET IsDeleted = 0 WHERE MaTN = @MaTN AND MaLoaiPhong = @MaLoaiPhong";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaTN", maTN);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", maLoaiPhong);

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

		public List<CT_TienNghi> getCTTienNghiByLoaiPhong(int maLoaiPhong)
		{
			var list = new List<CT_TienNghi>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
				SELECT CT.MaCTTN, CT.MaTN, CT.MaLoaiPhong, CT.SL, TN.TenTN
				FROM CT_TienNghi CT
				JOIN TienNghi TN ON CT.MaTN = TN.MaTN
				WHERE CT.MaLoaiPhong = @MaLoaiPhong";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaLoaiPhong", maLoaiPhong);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							list.Add(new CT_TienNghi
							{
								MaCTTN = reader.GetInt32("MaCTTN"),
								MaTN = reader.GetInt32("MaTN"),
								MaLoaiPhong = reader.GetInt32("MaLoaiPhong"),
								SL = reader.GetInt32("SL"),
								TenTN = reader["TenTN"].ToString()
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return list;
		}

	}
}