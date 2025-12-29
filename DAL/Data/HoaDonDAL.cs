using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class HoaDonDAL
	{
		private static HoaDonDAL Instance;

		private HoaDonDAL()
		{
		}

		public static HoaDonDAL GetInstance()
		{
			if (Instance == null) Instance = new HoaDonDAL();
			return Instance;
		}

		// Lấy tất cả các hóa đơn
		public List<HoaDon> LayDuLieuHoaDon()
		{
			var lstHoaDon = new List<HoaDon>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
					SELECT 
						hd.MaHD,
						hd.MaNV AS HD_MaNV,
						hd.MaCTPT AS HD_MaCTPT,
						hd.MaCTSDDV,
						hd.NgayLap,
						hd.TongTien,
						nv.MaNV AS NV_MaNV,
						nv.HoTen,
						nv.ChucVu,
						nv.SDT,
						nv.DiaChi, 
						nv.CCCD,
						nv.NTNS,
						nv.GioiTinh,
						nv.Luong,
						ctpt.MaCTPT AS CTPT_MaCTPT,
						ctpt.MaPhieuThue,
						ctpt.SoPhong,
						ctpt.NgayBD, 
						ctpt.NgayKT,
						ctpt.SoNguoiO,
						ctpt.TinhTrangThue,
						ctpt.TienPhong,
						ctpt.DaThanhToan
					FROM HoaDon hd
					INNER JOIN NhanVien nv ON hd.MaNV = nv.MaNV
					LEFT JOIN CT_PhieuThue ctpt ON hd.MaCTPT = ctpt.MaCTPT
					";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var hoaDon = new HoaDon
							{
								MaHD = reader.GetInt32(reader.GetOrdinal("MaHD")),
								MaNV = reader.GetInt32(reader.GetOrdinal("HD_MaNV")),
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("HD_MaCTPT"))
									? (int?)null
									: reader.GetInt32(reader.GetOrdinal("HD_MaCTPT")),
								MaCTSDDV = reader.IsDBNull(reader.GetOrdinal("MaCTSDDV"))
									? (int?)null
									: reader.GetInt32(reader.GetOrdinal("MaCTSDDV")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
								TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien")),
								NhanVien = reader.IsDBNull(reader.GetOrdinal("NV_MaNV"))
									? null
									: new NhanVien
									{
										MaNV = reader.GetInt32(reader.GetOrdinal("NV_MaNV")),
										HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
										ChucVu = reader.GetString(reader.GetOrdinal("ChucVu")),
										SDT = reader.GetString(reader.GetOrdinal("SDT")),
										DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
										CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
										NTNS = reader.GetDateTime(reader.GetOrdinal("NTNS")),
										GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
										Luong = reader.GetDecimal(reader.GetOrdinal("Luong"))
									},
								CT_PhieuThue = reader.IsDBNull(reader.GetOrdinal("CTPT_MaCTPT"))
									? null
									: new CT_PhieuThue
									{
										MaCTPT = reader.GetInt32(reader.GetOrdinal("CTPT_MaCTPT")),
										MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
										SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
										NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
										NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
										SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
										TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
										TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong")),
										DaThanhToan = reader.IsDBNull(reader.GetOrdinal("DaThanhToan"))
											? (bool?)null
											: reader.GetBoolean(reader.GetOrdinal("DaThanhToan"))
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

		public HoaDon LayHoaDonTheoMaCTPT(int? maCTPT)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
					SELECT 
						hd.MaHD,
						hd.MaNV AS HD_MaNV,
						hd.MaCTPT AS HD_MaCTPT,
						hd.MaCTSDDV,
						hd.NgayLap,
						hd.TongTien,
						nv.MaNV AS NV_MaNV,
						nv.HoTen,
						nv.ChucVu,
						nv.SDT,
						nv.DiaChi, 
						nv.CCCD,
						nv.NTNS,
						nv.GioiTinh,
						nv.Luong,
						ctpt.MaCTPT AS CTPT_MaCTPT,
						ctpt.MaPhieuThue,
						ctpt.SoPhong,
						ctpt.NgayBD, 
						ctpt.NgayKT,
						ctpt.SoNguoiO,
						ctpt.TinhTrangThue,
						ctpt.TienPhong,
						ctpt.DaThanhToan
					FROM HoaDon hd
					INNER JOIN NhanVien nv ON hd.MaNV = nv.MaNV
					LEFT JOIN CT_PhieuThue ctpt ON hd.MaCTPT = ctpt.MaCTPT
					WHERE hd.MaCTPT = @MaCTPT";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							return new HoaDon
							{
								MaHD = reader.GetInt32(reader.GetOrdinal("MaHD")),
								MaNV = reader.GetInt32(reader.GetOrdinal("HD_MaNV")),
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("HD_MaCTPT"))
									? (int?)null
									: reader.GetInt32(reader.GetOrdinal("HD_MaCTPT")),
								MaCTSDDV = reader.IsDBNull(reader.GetOrdinal("MaCTSDDV"))
									? (int?)null
									: reader.GetInt32(reader.GetOrdinal("MaCTSDDV")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
								TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien")),
								NhanVien = reader.IsDBNull(reader.GetOrdinal("NV_MaNV"))
									? null
									: new NhanVien
									{
										MaNV = reader.GetInt32(reader.GetOrdinal("NV_MaNV")),
										HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
										ChucVu = reader.GetString(reader.GetOrdinal("ChucVu")),
										SDT = reader.GetString(reader.GetOrdinal("SDT")),
										DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
										CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
										NTNS = reader.GetDateTime(reader.GetOrdinal("NTNS")),
										GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
										Luong = reader.GetDecimal(reader.GetOrdinal("Luong"))
									},
								CT_PhieuThue = reader.IsDBNull(reader.GetOrdinal("CTPT_MaCTPT"))
									? null
									: new CT_PhieuThue
									{
										MaCTPT = reader.GetInt32(reader.GetOrdinal("CTPT_MaCTPT")),
										MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
										SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
										NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
										NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
										SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
										TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
										TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong")),
										DaThanhToan = reader.IsDBNull(reader.GetOrdinal("DaThanhToan"))
											? (bool?)null
											: reader.GetBoolean(reader.GetOrdinal("DaThanhToan"))
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

		public HoaDon LayHoaDonTheoMaCTSDDV(int maCTSDDV)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
					SELECT 
						hd.MaHD,
						hd.MaNV AS HD_MaNV,
						hd.MaCTPT AS HD_MaCTPT,
						hd.MaCTSDDV,
						hd.NgayLap,
						hd.TongTien,
						nv.MaNV AS NV_MaNV,
						nv.HoTen,
						nv.ChucVu,
						nv.SDT,
						nv.DiaChi, 
						nv.CCCD,
						nv.NTNS,
						nv.GioiTinh,
						nv.Luong,
						ctpt.MaCTPT AS CTPT_MaCTPT,
						ctpt.MaPhieuThue,
						ctpt.SoPhong,
						ctpt.NgayBD, 
						ctpt.NgayKT,
						ctpt.SoNguoiO,
						ctpt.TinhTrangThue,
						ctpt.TienPhong,
						ctpt.DaThanhToan
					FROM HoaDon hd
					INNER JOIN NhanVien nv ON hd.MaNV = nv.MaNV
					LEFT JOIN CT_PhieuThue ctpt ON hd.MaCTPT = ctpt.MaCTPT
					WHERE hd.MaCTSDDV = @MaCTSDDV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTSDDV", maCTSDDV);

					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							return new HoaDon
							{
								MaHD = reader.GetInt32(reader.GetOrdinal("MaHD")),
								MaNV = reader.GetInt32(reader.GetOrdinal("HD_MaNV")),
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("HD_MaCTPT"))
									? (int?)null
									: reader.GetInt32(reader.GetOrdinal("HD_MaCTPT")),
								MaCTSDDV = reader.IsDBNull(reader.GetOrdinal("MaCTSDDV"))
									? (int?)null
									: reader.GetInt32(reader.GetOrdinal("MaCTSDDV")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
								TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien")),
								NhanVien = reader.IsDBNull(reader.GetOrdinal("NV_MaNV"))
									? null
									: new NhanVien
									{
										MaNV = reader.GetInt32(reader.GetOrdinal("NV_MaNV")),
										HoTen = reader.GetString(reader.GetOrdinal("HoTen")),
										ChucVu = reader.GetString(reader.GetOrdinal("ChucVu")),
										SDT = reader.GetString(reader.GetOrdinal("SDT")),
										DiaChi = reader.GetString(reader.GetOrdinal("DiaChi")),
										CCCD = reader.GetString(reader.GetOrdinal("CCCD")),
										NTNS = reader.GetDateTime(reader.GetOrdinal("NTNS")),
										GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
										Luong = reader.GetDecimal(reader.GetOrdinal("Luong"))
									},
								CT_PhieuThue = reader.IsDBNull(reader.GetOrdinal("CTPT_MaCTPT"))
									? null
									: new CT_PhieuThue
									{
										MaCTPT = reader.GetInt32(reader.GetOrdinal("CTPT_MaCTPT")),
										MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
										SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
										NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
										NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
										SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
										TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
										TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong")),
										DaThanhToan = reader.IsDBNull(reader.GetOrdinal("DaThanhToan"))
											? (bool?)null
											: reader.GetBoolean(reader.GetOrdinal("DaThanhToan"))
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

		// Thêm hóa đơn mới
		public bool themHoaDon(HoaDon hd, out string error)
		{
			error = string.Empty;
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        INSERT INTO HoaDon (MaCTPT, MaCTSDDV, MaNV, NgayLap, TongTien)
                        VALUES (@MaCTPT, @MaCTSDDV, @MaNV ,@NgayLap, @TongTien)";
					var cmd = new MySqlCommand(query, conn);

					cmd.Parameters.AddWithValue("@MaCTPT", hd.MaCTPT);
					cmd.Parameters.AddWithValue("@MaCTSDDV", hd.MaCTSDDV);
					cmd.Parameters.AddWithValue("@MaNV", hd.MaNV);
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

		public int layMaHDMoiNhat()
		{
			var maHD = -1; // Giá trị mặc định nếu không tìm thấy
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT MAX(MaHD) AS MaHD FROM HoaDon";
					var cmd = new MySqlCommand(query, conn);

					conn.Open();
					var result = cmd.ExecuteScalar();

					if (result != null && result != DBNull.Value) maHD = Convert.ToInt32(result);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi lấy MaKH mới nhất: " + ex.Message);
			}

			return maHD;
		}

		public bool capNhatHoaDon(HoaDon hd)
		{
			var connectionString = Properties.Resources.MySqlConnection;
			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "UPDATE HoaDon SET MaNV = @MaNV, NgayLap = @NgayLap, TongTien=@TongTien WHERE MaHD = @MaHD";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaNV", hd.MaNV);
					cmd.Parameters.AddWithValue("@NgayLap", hd.NgayLap);
					cmd.Parameters.AddWithValue("@TongTien", hd.TongTien);
					cmd.Parameters.AddWithValue("@MaHD", hd.MaHD);

					conn.Open();
					var rowsAffected = cmd.ExecuteNonQuery();
					return rowsAffected > 0;
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
