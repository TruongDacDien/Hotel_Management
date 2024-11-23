using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "SELECT * FROM NhanVien";
					SqlCommand cmd = new SqlCommand(query, conn);

					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
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
								MaTK = reader.GetInt32(reader.GetOrdinal("MaTK"))
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
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = @"
                        INSERT INTO NhanVien (HoTen, ChucVu, SDT, DiaChi, CCCD, NTNS, GioiTinh, Luong, MaTK)
                        VALUES (@HoTen, @ChucVu, @SDT, @DiaChi, @CCCD, @NTNS, @GioiTinh, @Luong, @MaTK)";

					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
					cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
					cmd.Parameters.AddWithValue("@SDT", nv.SDT);
					cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
					cmd.Parameters.AddWithValue("@CCCD", nv.CCCD);
					cmd.Parameters.AddWithValue("@NTNS", nv.NTNS);
					cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
					cmd.Parameters.AddWithValue("@Luong", nv.Luong);
					cmd.Parameters.AddWithValue("@MaTK", nv.MaTK);

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
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = @"
                        UPDATE NhanVien
                        SET HoTen = @HoTen, ChucVu = @ChucVu, SDT = @SDT, DiaChi = @DiaChi, 
                            CCCD = @CCCD, NTNS = @NTNS, GioiTinh = @GioiTinh, Luong = @Luong, MaTK = @MaTK
                        WHERE MaNV = @MaNV";

					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaNV", nv.MaNV);
					cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
					cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
					cmd.Parameters.AddWithValue("@SDT", nv.SDT);
					cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
					cmd.Parameters.AddWithValue("@CCCD", nv.CCCD);
					cmd.Parameters.AddWithValue("@NTNS", nv.NTNS);
					cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
					cmd.Parameters.AddWithValue("@Luong", nv.Luong);
					cmd.Parameters.AddWithValue("@MaTK", nv.MaTK);

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
		public bool deleteDataNhanVien(int maNV)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "DELETE FROM NhanVien WHERE MaNV = @MaNV";

					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaNV", maNV);

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

		// Lấy thông tin nhân viên theo mã
		public NhanVien layNhanVienTheoMaNV(int maNV)
		{
			NhanVien nhanVien = null;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					string query = "SELECT * FROM NhanVien WHERE MaNV = @MaNV";
					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaNV", maNV);

					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
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
								MaTK = reader.GetInt32(reader.GetOrdinal("MaTK"))
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
