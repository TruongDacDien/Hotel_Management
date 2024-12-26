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
						hd.MaHD, hd.MaNV, hd.MaCTPT, hd.NgayLap, hd.TongTien,
						nv.MaNV, nv.HoTen, nv.ChucVu, nv.SDT, nv.DiaChi, 
						nv.CCCD, nv.NTNS, nv.GioiTinh, nv.Luong,
						ctpt.MaCTPT, ctpt.MaPhieuThue, ctpt.SoPhong, ctpt.NgayBD, 
						ctpt.NgayKT, ctpt.SoNguoiO, ctpt.TinhTrangThue, ctpt.TienPhong
					FROM HoaDon hd, NhanVien nv, CT_PhieuThue ctpt
					WHERE hd.MaNV = nv.MaNV AND hd.MaCTPT = ctpt.MaCTPT AND ctpt.TinhTrangThue = 'Đã thanh toán'";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var hoaDon = new HoaDon
							{
								MaHD = reader.GetInt32(reader.GetOrdinal("MaHD")),
								MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT"))
									? (int?)null
									: reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
								TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien")),
								NhanVien = reader.IsDBNull(reader.GetOrdinal("MaNV"))
									? null
									: new NhanVien
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
									},
								CT_PhieuThue = reader.IsDBNull(reader.GetOrdinal("MaCTPT"))
									? null
									: new CT_PhieuThue
									{
										MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
										MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
										SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
										NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
										NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
										SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
										TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
										TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong"))
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
						hd.MaHD, hd.MaNV, hd.MaCTPT, hd.NgayLap, hd.TongTien,
						nv.MaNV, nv.HoTen, nv.ChucVu, nv.SDT, nv.DiaChi, 
						nv.CCCD, nv.NTNS, nv.GioiTinh, nv.Luong,
						ctpt.MaCTPT, ctpt.MaPhieuThue, ctpt.SoPhong, ctpt.NgayBD, 
						ctpt.NgayKT, ctpt.SoNguoiO, ctpt.TinhTrangThue, ctpt.TienPhong
					FROM HoaDon hd, NhanVien nv, CT_PhieuThue ctpt
					WHERE hd.MaNV = nv.MaNV AND hd.MaCTPT = ctpt.MaCTPT AND hd.MaCTPT = @MaCTPT";
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
								MaNV = reader.GetInt32(reader.GetOrdinal("MaNV")),
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT"))
									? (int?)null
									: reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
								TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien")),
								NhanVien = reader.IsDBNull(reader.GetOrdinal("MaNV"))
									? null
									: new NhanVien
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
									},
								CT_PhieuThue = reader.IsDBNull(reader.GetOrdinal("MaCTPT"))
									? null
									: new CT_PhieuThue
									{
										MaCTPT = reader.GetInt32(reader.GetOrdinal("MaCTPT")),
										MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
										SoPhong = reader.GetString(reader.GetOrdinal("SoPhong")),
										NgayBD = reader.GetDateTime(reader.GetOrdinal("NgayBD")),
										NgayKT = reader.GetDateTime(reader.GetOrdinal("NgayKT")),
										SoNguoiO = reader.GetInt32(reader.GetOrdinal("SoNguoiO")),
										TinhTrangThue = reader.GetString(reader.GetOrdinal("TinhTrangThue")),
										TienPhong = reader.GetDecimal(reader.GetOrdinal("TienPhong"))
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
                        INSERT INTO HoaDon (MaCTPT, MaNV, NgayLap, TongTien)
                        VALUES (@MaCTPT, @MaNV ,@NgayLap, @TongTien)";
					var cmd = new MySqlCommand(query, conn);

					cmd.Parameters.AddWithValue("@MaCTPT", hd.MaCTPT);
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