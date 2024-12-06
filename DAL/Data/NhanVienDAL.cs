using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using DAL.DTO;

namespace DAL.Data
{
	public class NhanVienDAL
	{
		private static NhanVienDAL Instance;

		private NhanVienDAL() { }

		public static NhanVienDAL GetInstance()
		{
			if (Instance == null)
			{
				Instance = new NhanVienDAL();
			}
			return Instance;
		}

		// Lấy danh sách nhân viên từ DB
		public List<NhanVien> getDataNhanVien()
		{
			List<NhanVien> lsNV = new List<NhanVien>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT * FROM NhanVien WHERE IsDeleted = 0";
					MySqlCommand cmd = new MySqlCommand(query, conn);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							lsNV.Add(new NhanVien
							{
								MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
								HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
								ChucVu = reader.GetString(reader.GetOrdinal("ChucVu")),
								SDT = reader.GetString(reader.GetOrdinal("SDT")),
								DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
								CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
								NTNS = reader.GetDateTime(reader.GetOrdinal("NTNS")),
								GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
								Luong = reader.GetDecimal(reader.GetOrdinal("Luong")),
								IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return lsNV;
		}

		// Thêm mới nhân viên
		public bool addDataNhanVien(NhanVien nv)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        INSERT INTO NhanVien (HoTen, ChucVu, SDT, DiaChi, CCCD, NTNS, GioiTinh, Luong, IsDeleted)
                        VALUES (@HoTen, @ChucVu, @SDT, @DiaChi, @CCCD, @NTNS, @GioiTinh, @Luong, 0)";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
					cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
					cmd.Parameters.AddWithValue("@SDT", nv.SDT);
					cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
					cmd.Parameters.AddWithValue("@CCCD", nv.CCCD);
					cmd.Parameters.AddWithValue("@NTNS", nv.NTNS);
					cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
					cmd.Parameters.AddWithValue("@Luong", nv.Luong);

					conn.Open();
					cmd.ExecuteNonQuery();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		// Sửa thông tin nhân viên
		public bool updateDataNhanVien(NhanVien nv)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        UPDATE NhanVien
                        SET HoTen = @HoTen, ChucVu = @ChucVu, SDT = @SDT, DiaChi = @DiaChi, 
                            CCCD = @CCCD, NTNS = @NTNS, GioiTinh = @GioiTinh, Luong = @Luong
                        WHERE MaNV = @MaNV";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaNV", nv.MaNV);
					cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
					cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
					cmd.Parameters.AddWithValue("@SDT", nv.SDT);
					cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
					cmd.Parameters.AddWithValue("@CCCD", nv.CCCD);
					cmd.Parameters.AddWithValue("@NTNS", nv.NTNS);
					cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
					cmd.Parameters.AddWithValue("@Luong", nv.Luong);

					conn.Open();
					cmd.ExecuteNonQuery();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		// Xóa nhân viên
		public bool deleteDataNhanVien(NhanVien nv)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "UPDATE NhanVien SET IsDeleted = 1 WHERE MaNV = @MaNV";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaNV", nv.MaNV);
					conn.Open();
					cmd.ExecuteNonQuery();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		// Kiểm tra tồn tại nhân viên dựa trên CCCD
		public NhanVien kiemTraTonTaiNhanVien(string CCCD)
		{
			NhanVien nhanVien = null;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT * FROM NhanVien WHERE CCCD = @CCCD";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@CCCD", CCCD);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							nhanVien = new NhanVien
							{
								MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
								HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
								ChucVu = reader.GetString(reader.GetOrdinal("ChucVu")),
								SDT = reader.GetString(reader.GetOrdinal("SDT")),
								DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
								CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
								NTNS = reader.GetDateTime(reader.GetOrdinal("NTNS")),
								GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
								Luong = reader.GetDecimal(reader.GetOrdinal("Luong")),
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
			return nhanVien;
		}

		// Lấy thông tin nhân viên theo mã
		public NhanVien layNhanVienTheoMaNV(int maNV)
		{
			NhanVien nhanVien = null;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = "SELECT * FROM NhanVien WHERE MaNV = @MaNV AND IsDeleted = 0";
					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaNV", maNV);

					conn.Open();
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							nhanVien = new NhanVien
							{
								MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
								HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
								ChucVu = reader.GetString(reader.GetOrdinal("ChucVu")),
								SDT = reader.GetString(reader.GetOrdinal("SDT")),
								DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
								CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
								NTNS = reader.GetDateTime(reader.GetOrdinal("NTNS")),
								GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
								Luong = reader.GetDecimal(reader.GetOrdinal("Luong"))
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return nhanVien;
		}
	}
}
