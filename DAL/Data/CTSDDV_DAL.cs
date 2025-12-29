using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class CTSDDV_DAL
	{
		private static CTSDDV_DAL Instance;

		private CTSDDV_DAL()
		{
		}

		public static CTSDDV_DAL GetInstance()
		{
			if (Instance == null) Instance = new CTSDDV_DAL();
			return Instance;
		}

		// Thêm mới chi tiết sử dụng dịch vụ
		public bool addDataCTSDDC(CT_SDDichVu ctsddv, out string error)
		{
			error = string.Empty;
			var connectionString = Properties.Resources.MySqlConnection;
			var soLuong = ctsddv.SL;
			if (soLuong <= 0)
			{
				error = "So luong dich vu khong hop le.";
				return false;
			}

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					conn.Open();
					using (var tran = conn.BeginTransaction())
					{
						var insertQuery = @"INSERT INTO CT_SDDichVu 
                                (MaCTPT, MaKH, MaDV, SL, ThanhTien, NgayApDung, TrangThai, DaThanhToan, HinhThucThanhToan) 
                                VALUES 
                                (@MaCTPT, @MaKH, @MaDV, @SL, @ThanhTien, @NgayApDung, @TrangThai, @DaThanhToan, @HinhThucThanhToan)";
						var insertCmd = new MySqlCommand(insertQuery, conn, tran);
						insertCmd.Parameters.AddWithValue("@MaCTPT", ctsddv.MaCTPT);
						insertCmd.Parameters.AddWithValue("@MaKH", ctsddv.MaKH);
						insertCmd.Parameters.AddWithValue("@MaDV", ctsddv.MaDV);
						insertCmd.Parameters.AddWithValue("@SL", ctsddv.SL);
						insertCmd.Parameters.AddWithValue("@ThanhTien", ctsddv.ThanhTien);
						insertCmd.Parameters.AddWithValue("@NgayApDung", ctsddv.NgayApDung);
						insertCmd.Parameters.AddWithValue("@TrangThai", ctsddv.TrangThai);
						insertCmd.Parameters.AddWithValue("@DaThanhToan",
							ctsddv.DaThanhToan.HasValue && ctsddv.DaThanhToan.Value);
						insertCmd.Parameters.AddWithValue("@HinhThucThanhToan", ctsddv.HinhThucThanhToan);

						insertCmd.ExecuteNonQuery();

						var updateQuery = @"UPDATE DichVu
								SET SoLuong = SoLuong - @SL
								WHERE MaDV = @MaDV AND SoLuong >= @SL";
						var updateCmd = new MySqlCommand(updateQuery, conn, tran);
						updateCmd.Parameters.AddWithValue("@MaDV", ctsddv.MaDV);
						updateCmd.Parameters.AddWithValue("@SL", ctsddv.SL);

						if (updateCmd.ExecuteNonQuery() == 0)
						{
							tran.Rollback();
							error = "So luong dich vu khong du.";
							return false;
						}

						tran.Commit();
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

		// Tính tổng tiền chi tiết sử dụng dịch vụ theo mã CTPT
		public List<decimal> tongTienChiTietSuDungDichVu(int? maCTPT)
		{
			var results = new List<decimal>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"SELECT ThanhTien FROM CT_SDDichVu WHERE MaCTPT = @MaCTPT";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read()) results.Add(reader.GetDecimal(reader.GetOrdinal("ThanhTien")));
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return results;
		}

		// Lấy danh sách sử dụng dịch vụ của 1 phòng dựa vào mã CTPT
		public List<DichVu_DaChon> getCTSDDVtheoMaCTPT(int? maCTPT)
		{
			var ls = new List<DichVu_DaChon>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        SELECT 
                            ct.ThanhTien, 
                            ct.MaDV, 
                            dv.TenDV, 
                            ct.SL, 
                            dv.Gia
                        FROM CT_SDDichVu ct
                        INNER JOIN DichVu dv ON ct.MaDV = dv.MaDV
                        WHERE ct.MaCTPT = @MaCTPT";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							ls.Add(new DichVu_DaChon
							{
								ThanhTien = reader.GetDecimal(reader.GetOrdinal("ThanhTien")),
								MaDV = reader.GetInt32(reader.GetOrdinal("MaDV")),
								TenDV = reader.GetString(reader.GetOrdinal("TenDV")),
								SoLuong = reader.GetInt32(reader.GetOrdinal("SL")),
								Gia = reader.GetDecimal(reader.GetOrdinal("Gia"))
							});
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message); // Log lỗi nếu cần
			}

			return ls;
		}

		// Lay chi tiet CT_SDDichVu theo ma
		public CT_SDDichVu getCTSDDVTheoMaCTSDDV(int maCTSDDV)
		{
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        SELECT 
                            ct.MaCTSDDV,
                            ct.MaCTPT,
                            ct.MaKH,
                            ct.MaDV,
                            ct.SL,
                            ct.ThanhTien,
                            ct.NgayApDung,
                            ct.TrangThai,
                            ct.DaThanhToan,
                            ct.HinhThucThanhToan,
                            kh.TenKH,
                            dv.TenDV,
                            ctpt.SoPhong
                        FROM CT_SDDichVu ct
                        INNER JOIN KhachHang kh ON ct.MaKH = kh.MaKH
                        INNER JOIN DichVu dv ON ct.MaDV = dv.MaDV
                        LEFT JOIN CT_PhieuThue ctpt ON ct.MaCTPT = ctpt.MaCTPT
                        WHERE ct.MaCTSDDV = @MaCTSDDV";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@MaCTSDDV", maCTSDDV);

					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							return new CT_SDDichVu
							{
								MaCTSDDV = reader.GetInt32(reader.GetOrdinal("MaCTSDDV")),
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT"))
									? null
									: (int?)reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								MaKH = reader.GetInt32(reader.GetOrdinal("MaKH")),
								MaDV = reader.GetInt32(reader.GetOrdinal("MaDV")),
								SL = reader.GetInt32(reader.GetOrdinal("SL")),
								ThanhTien = reader.GetDecimal(reader.GetOrdinal("ThanhTien")),
								NgayApDung = reader.GetDateTime(reader.GetOrdinal("NgayApDung")),
								TrangThai = reader.IsDBNull(reader.GetOrdinal("TrangThai"))
									? null
									: reader.GetString(reader.GetOrdinal("TrangThai")),
								DaThanhToan = reader.IsDBNull(reader.GetOrdinal("DaThanhToan"))
									? (bool?)null
									: reader.GetBoolean(reader.GetOrdinal("DaThanhToan")),
								HinhThucThanhToan = reader.IsDBNull(reader.GetOrdinal("HinhThucThanhToan"))
									? null
									: reader.GetString(reader.GetOrdinal("HinhThucThanhToan")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								TenDV = reader.GetString(reader.GetOrdinal("TenDV")),
								SoPhong = reader.IsDBNull(reader.GetOrdinal("SoPhong"))
									? null
									: reader.GetString(reader.GetOrdinal("SoPhong"))
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return null;
		}

		// Lay danh sach CT_SDDichVu de quan ly
		public List<CT_SDDichVu> getCTSDDVQuanLy()
		{
			var ls = new List<CT_SDDichVu>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        SELECT 
                            ct.MaCTSDDV,
                            ct.MaCTPT,
                            ct.MaKH,
                            ct.MaDV,
                            ct.SL,
                            ct.ThanhTien,
                            ct.NgayApDung,
                            ct.TrangThai,
                            ct.DaThanhToan,
                            ct.HinhThucThanhToan,
                            kh.TenKH,
                            dv.TenDV,
                            ctpt.SoPhong
                        FROM CT_SDDichVu ct
                        INNER JOIN KhachHang kh ON ct.MaKH = kh.MaKH
                        INNER JOIN DichVu dv ON ct.MaDV = dv.MaDV
                        LEFT JOIN CT_PhieuThue ctpt ON ct.MaCTPT = ctpt.MaCTPT
                        ORDER BY ct.NgayApDung DESC";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							ls.Add(new CT_SDDichVu
							{
								MaCTSDDV = reader.GetInt32(reader.GetOrdinal("MaCTSDDV")),
								MaCTPT = reader.IsDBNull(reader.GetOrdinal("MaCTPT"))
									? null
									: (int?)reader.GetInt32(reader.GetOrdinal("MaCTPT")),
								MaKH = reader.GetInt32(reader.GetOrdinal("MaKH")),
								MaDV = reader.GetInt32(reader.GetOrdinal("MaDV")),
								SL = reader.GetInt32(reader.GetOrdinal("SL")),
								ThanhTien = reader.GetDecimal(reader.GetOrdinal("ThanhTien")),
								NgayApDung = reader.GetDateTime(reader.GetOrdinal("NgayApDung")),
								TrangThai = reader.IsDBNull(reader.GetOrdinal("TrangThai"))
									? null
									: reader.GetString(reader.GetOrdinal("TrangThai")),
								DaThanhToan = reader.IsDBNull(reader.GetOrdinal("DaThanhToan"))
									? (bool?)null
									: reader.GetBoolean(reader.GetOrdinal("DaThanhToan")),
								HinhThucThanhToan = reader.IsDBNull(reader.GetOrdinal("HinhThucThanhToan"))
									? null
									: reader.GetString(reader.GetOrdinal("HinhThucThanhToan")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								TenDV = reader.GetString(reader.GetOrdinal("TenDV")),
								SoPhong = reader.IsDBNull(reader.GetOrdinal("SoPhong"))
									? null
									: reader.GetString(reader.GetOrdinal("SoPhong"))
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return ls;
		}

		public bool capNhatTrangThaiSuDung(int maCTSDDV, string trangThai, bool daThanhToan, string hinhThucThanhToan,
			out string error)
		{
			error = string.Empty;
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        UPDATE CT_SDDichVu 
                        SET TrangThai = @TrangThai, DaThanhToan = @DaThanhToan, HinhThucThanhToan = @HinhThucThanhToan
                        WHERE MaCTSDDV = @MaCTSDDV";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@TrangThai", trangThai);
					cmd.Parameters.AddWithValue("@DaThanhToan", daThanhToan);
					cmd.Parameters.AddWithValue("@HinhThucThanhToan", hinhThucThanhToan);
					cmd.Parameters.AddWithValue("@MaCTSDDV", maCTSDDV);

					conn.Open();
					return cmd.ExecuteNonQuery() > 0;
				}
			}
			catch (Exception ex)
			{
				error = ex.Message;
				return false;
			}
		}

		public bool capNhatThanhToanTheoMaCTPT(int? maCTPT, bool daThanhToan, string hinhThucThanhToan, out string error)
		{
			error = string.Empty;
			if (maCTPT == null)
			{
				error = "MaCTPT is null.";
				return false;
			}

			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        UPDATE CT_SDDichVu 
                        SET DaThanhToan = @DaThanhToan, HinhThucThanhToan = @HinhThucThanhToan
                        WHERE MaCTPT = @MaCTPT";
					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@DaThanhToan", daThanhToan);
					cmd.Parameters.AddWithValue("@HinhThucThanhToan", hinhThucThanhToan);
					cmd.Parameters.AddWithValue("@MaCTPT", maCTPT);

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
