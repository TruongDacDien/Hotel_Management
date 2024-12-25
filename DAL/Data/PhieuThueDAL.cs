using System;
using System.Collections.Generic;
using System.Configuration;
using DAL.DTO;
using MySql.Data.MySqlClient;

namespace DAL.Data
{
	public class PhieuThueDAL
	{
		private static PhieuThueDAL Instance;

		private PhieuThueDAL()
		{
		}

		public static PhieuThueDAL GetInstance()
		{
			if (Instance == null) Instance = new PhieuThueDAL();
			return Instance;
		}

		// Thêm mới phiếu thuê
		public bool addPhieuThue(PhieuThue pt, out string error)
		{
			error = string.Empty;
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        INSERT INTO PhieuThue (NgayLapPhieu, MaKH, MaNV, IsDeleted)
                        VALUES (@NgayLapPhieu, @MaKH, @MaNV, 0)";

					var cmd = new MySqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@NgayLapPhieu", pt.NgayLapPhieu);
					cmd.Parameters.AddWithValue("@MaKH", pt.MaKH);
					cmd.Parameters.AddWithValue("@MaNV", pt.MaNV);

					conn.Open();
					cmd.ExecuteNonQuery();
				}

				return true;
			}
			catch (Exception e)
			{
				error = e.Message;
				return false;
			}
		}

		// Xóa phiếu thuê theo mã phiếu thuê
		public bool xoaPhieuThueTheoMaPhieuThue(int maPhieuThue, out string error)
		{
			error = string.Empty;
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					// Truy vấn kiểm tra TinhTrangThue của phiếu thuê
					var queryTinhTrang =
						"SELECT TinhTrangThue FROM CT_PhieuThue WHERE MaPhieuThue = @MaPhieuThue LIMIT 1";
					var cmdTinhTrang = new MySqlCommand(queryTinhTrang, conn);
					cmdTinhTrang.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);

					conn.Open();
					var tinhTrangThue = cmdTinhTrang.ExecuteScalar()?.ToString();

					if (string.IsNullOrEmpty(tinhTrangThue))
					{
						error = "Không tồn tại phiếu thuê với mã " + maPhieuThue;
						return false;
					}

					if (tinhTrangThue == "Đã trả phòng" || tinhTrangThue == "Phòng đang thuê")
					{
						// Cập nhật IsDeleted = 1 cho phiếu thuê trong PhieuThue
						var updatePhieuThueQuery =
							"UPDATE PhieuThue SET IsDeleted = 1 WHERE MaPhieuThue = @MaPhieuThue";
						var updatePhieuThueCmd = new MySqlCommand(updatePhieuThueQuery, conn);
						updatePhieuThueCmd.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);
						updatePhieuThueCmd.ExecuteNonQuery();
					}
					else
					{
						// Nếu không phải 2 trạng thái trên, xóa luôn các bản ghi trong CT_PhieuThue và sau đó xóa phiếu thuê trong PhieuThue
						var deleteCTQuery = "DELETE FROM CT_PhieuThue WHERE MaPhieuThue = @MaPhieuThue";
						var deleteCTCmd = new MySqlCommand(deleteCTQuery, conn);
						deleteCTCmd.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);
						deleteCTCmd.ExecuteNonQuery();

						// Xóa phiếu thuê trong bảng PhieuThue
						var deletePhieuThueQuery = "DELETE FROM PhieuThue WHERE MaPhieuThue = @MaPhieuThue";
						var deletePhieuThueCmd = new MySqlCommand(deletePhieuThueQuery, conn);
						deletePhieuThueCmd.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);
						deletePhieuThueCmd.ExecuteNonQuery();
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


		// Lấy dữ liệu phiếu thuê từ DB
		public List<PhieuThue_Custom> getDataFromDB()
		{
			var ls = new List<PhieuThue_Custom>();
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = @"
                        SELECT pt.MaPhieuThue, pt.NgayLapPhieu, kh.TenKH, nv.HoTen
                        FROM PhieuThue pt
                        JOIN KhachHang kh ON pt.MaKH = kh.MaKH
                        JOIN NhanVien nv ON pt.MaNV = nv.MaNV
						WHERE pt.IsDeleted = 0";

					var cmd = new MySqlCommand(query, conn);
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							ls.Add(new PhieuThue_Custom
							{
								MaPhieuThue = reader.GetInt32(reader.GetOrdinal("MaPhieuThue")),
								NgayLapPhieu = reader.GetDateTime(reader.GetOrdinal("NgayLapPhieu")),
								TenKH = reader.GetString(reader.GetOrdinal("TenKH")),
								TenNV = reader.GetString(reader.GetOrdinal("HoTen"))
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

		public int layMaPhieuThueMoiNhat()
		{
			var maPhieuThue = -1; // Giá trị mặc định nếu không tìm thấy
			var connectionString = Properties.Resources.MySqlConnection;

			try
			{
				using (var conn = new MySqlConnection(connectionString))
				{
					var query = "SELECT MAX(MaPhieuThue) AS MaPhieuThue FROM PhieuThue";
					var cmd = new MySqlCommand(query, conn);

					conn.Open();
					var result = cmd.ExecuteScalar();

					if (result != null && result != DBNull.Value) maPhieuThue = Convert.ToInt32(result);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi lấy MaKH mới nhất: " + ex.Message);
			}

			return maPhieuThue;
		}
	}
}