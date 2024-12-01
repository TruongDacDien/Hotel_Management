using DAL.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DAL.Data
{
	public class HoaDonDAL
	{
		private static HoaDonDAL Instance;

		private HoaDonDAL() { }

		public static HoaDonDAL GetInstance()
		{
			if (Instance == null)
			{
				Instance = new HoaDonDAL();
			}
			return Instance;
		}

		// Lấy tất cả các hóa đơn
		public List<HoaDonDTO> LayDuLieuHoaDon()
		{
			List<HoaDonDTO> lstHoaDon = new List<HoaDonDTO>();
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        SELECT 
                            hd.MaHD, hd.MaNV, hd.MaCTPT, hd.NgayLap, hd.TongTien,
                            nv.MaNV AS NV_MaNV, nv.HoTen, nv.ChucVu, nv.SDT, nv.DiaChi, 
                            nv.CCCD, nv.NTNS, nv.GioiTinh, nv.Luong, nv.MaTK,
                            ctpt.MaCTPT, ctpt.MaPhieuThue, ctpt.SoPhong, ctpt.NgayBD, 
                            ctpt.NgayKT, ctpt.SoNguoiO, ctpt.TinhTrangThue, ctpt.TienPhong, ctpt.NgayTraThucTe
                        FROM HoaDon hd
                        LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                        LEFT JOIN CT_PhieuThue ctpt ON hd.MaCTPT = ctpt.MaCTPT";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							HoaDonDTO hoaDon = new HoaDonDTO
							{
								MaHD = reader.GetInt32(reader.GetOrdinal("MaHD")),
								MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
								TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien")),
								NhanVien = new NhanVien
								{
									MaNV = reader.GetInt32(reader.GetOrdinal("NV_MaNV")),
									HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
									ChucVu = reader.GetString(reader.GetOrdinal("ChucVu")),
									SDT = reader.GetString(reader.GetOrdinal("SDT")),
									DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
									CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
									NTNS = reader.GetDateTime(reader.GetOrdinal("NTNS")),
									GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
									Luong = reader.GetDecimal(reader.GetOrdinal("Luong")),
								},
								CT_PhieuThue = reader.IsDBNull(reader.GetOrdinal("MaCTPT")) ? null : new CT_PhieuThue
								{
									MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
									MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
									SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
									NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
									NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
									SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
									TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
									TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong")),
									NgayTraThucTe = reader.GetDateTime(reader.GetOrdinal("NgayTraThucTe"))
								}
							};

							lstHoaDon.Add(hoaDon);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi: " + ex.Message);
			}

			return lstHoaDon;
		}

		// Lấy thông tin hóa đơn theo mã hóa đơn
		public HoaDon layHoaDonTheoMaHoaDon(int mahd)
		{
			HoaDon hoaDon = null;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        SELECT 
                            hd.MaHD, hd.MaNV, hd.MaCTPT, hd.NgayLap, hd.TongTien,
                            nv.HoTen AS TenNhanVien, nv.ChucVu, nv.SDT, nv.DiaChi, nv.CCCD, nv.NTNS, nv.GioiTinh, nv.Luong, nv.MaTK,
                            ctpt.MaPhieuThue, ctpt.SoPhong, ctpt.NgayBD, ctpt.NgayKT, ctpt.SoNguoiO, ctpt.TinhTrangThue, ctpt.TienPhong, ctpt.NgayTraThucTe
                        FROM HoaDon hd
                        LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                        LEFT JOIN CT_PhieuThue ctpt ON hd.MaCTPT = ctpt.MaCTPT
                        WHERE hd.MaHD = @MaHD";

					MySqlCommand cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaHD", mahd);
					conn.Open();

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							hoaDon = new HoaDon
							{
								MaHD = reader.GetInt32(reader.GetOrdinal("MaHD")),
								MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
								MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
								TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien")),
								NhanVien = new NhanVien
								{
									MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
									HoTen = reader.GetString(reader.GetOrdinal("TenNhanVien")),
									ChucVu = reader.GetString(reader.GetOrdinal("ChucVu")),
									SDT = reader.GetString(reader.GetOrdinal("SDT")),
									DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
									CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
									NTNS = reader.GetDateTime(reader.GetOrdinal("NTNS")),
									GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
									Luong = reader.GetDecimal(reader.GetOrdinal("Luong")),
								},
								CT_PhieuThue = reader.IsDBNull(reader.GetOrdinal("MaCTPT")) ? null : new CT_PhieuThue
								{
									MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
									MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
									SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
									NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
									NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
									SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
									TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
									TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong")),
									NgayTraThucTe = reader.GetDateTime(reader.GetOrdinal("NgayTraThucTe"))
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

			return hoaDon;
		}

		// Thêm hóa đơn mới
		public bool themHoaDon(HoaDon hd, out string error)
		{
			error = string.Empty;
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

			try
			{
				using (MySqlConnection conn = new MySqlConnection(connectionString))
				{
					string query = @"
                        INSERT INTO HoaDon (MaHD, MaPhieuThue, NgayLap, TongTien)
                        VALUES (@MaHD, @MaPhieuThue, @NgayLap, @TongTien)";
					MySqlCommand cmd = new MySqlCommand(query, conn);

					cmd.Parameters.AddWithValue("@MaHD", hd.MaHD);
					cmd.Parameters.AddWithValue("@MaPhieuThue", hd.MaCTPT);
					cmd.Parameters.AddWithValue("@NgayLap", hd.NgayLap);
					cmd.Parameters.AddWithValue("@TongTien", hd.TongTien);

					conn.Open();
					cmd.ExecuteNonQuery();
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
