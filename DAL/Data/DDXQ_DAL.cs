using DAL.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class DDXQ_DAL
    {
        private static DDXQ_DAL Instance;

        private DDXQ_DAL() { }

        public static DDXQ_DAL GetInstance()
        {
            if (Instance == null) Instance = new DDXQ_DAL();
            return Instance;
        }

        public List<DDXQ> GetAllDDXQ()
        {
            var list = new List<DDXQ>();
            var connectionString = Properties.Resources.MySqlConnection;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    var query = "SELECT * FROM DiaDiemXungQuanh ";
                    var cmd = new MySqlCommand(query, conn);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DDXQ
                            {
                                MaDD = Convert.ToInt32(reader["MaDD"]),
                                MaCN = Convert.ToInt32(reader["MaCN"]),
                                TenDD = reader["TenDD"].ToString(),
                                LoaiDD = reader["LoaiDD"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                DanhGia = Convert.ToDouble(reader["DanhGia"]),
                                ViDo = Convert.ToDouble(reader["ViDo"]),
                                KinhDo = Convert.ToDouble(reader["KinhDo"]),
                                KhoangCach = Convert.ToDouble(reader["KhoangCach"]),
                                ThoiGianDiChuyen = reader["ThoiGianDiChuyen"].ToString(),
                                ThoiGianCapNhat = Convert.ToDateTime(reader["ThoiGianCapNhat"])
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

        // Lấy danh sách theo MaDD
        public DDXQ GetDDXQByMaDD(int maDD)
        {
            DDXQ ddxq = null;
            var connectionString = Properties.Resources.MySqlConnection;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    var query = "SELECT * FROM DiaDiemXungQuanh WHERE MaDD = @MaDD";
                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaDD", maDD);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ddxq = new DDXQ
                            {
                                MaDD = Convert.ToInt32(reader["MaDD"]),
                                MaCN = Convert.ToInt32(reader["MaCN"]),
                                TenDD = reader["TenDD"].ToString(),
                                LoaiDD = reader["LoaiDD"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                DanhGia = Convert.ToDouble(reader["DanhGia"]),
                                ViDo = Convert.ToDouble(reader["ViDo"]),
                                KinhDo = Convert.ToDouble(reader["KinhDo"]),
                                KhoangCach = Convert.ToDouble(reader["KhoangCach"]),
                                ThoiGianDiChuyen = reader["ThoiGianDiChuyen"].ToString(),
                                ThoiGianCapNhat = Convert.ToDateTime(reader["ThoiGianCapNhat"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ddxq;
        }

        // Thêm DDXQ mới
        public bool AddDDXQ(DDXQ ddxq)
        {
            var connectionString = Properties.Resources.MySqlConnection;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    var query = @"
                        INSERT INTO DiaDiemXungQuanh 
                        (MaCN, TenDD, LoaiDD, DiaChi, DanhGia, ViDo, KinhDo, KhoangCach, ThoiGianDiChuyen, ThoiGianCapNhat)
                        VALUES 
                        (@MaCN, @TenDD, @LoaiDD, @DiaChi, @DanhGia, @ViDo, @KinhDo, @KhoangCach, @ThoiGianDiChuyen, @ThoiGianCapNhat)";
                    var cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@MaCN", ddxq.MaCN);
                    cmd.Parameters.AddWithValue("@TenDD", ddxq.TenDD);
                    cmd.Parameters.AddWithValue("@LoaiDD", ddxq.LoaiDD);
                    cmd.Parameters.AddWithValue("@DiaChi", ddxq.DiaChi);
                    cmd.Parameters.AddWithValue("@DanhGia", ddxq.DanhGia);
                    cmd.Parameters.AddWithValue("@ViDo", ddxq.ViDo);
                    cmd.Parameters.AddWithValue("@KinhDo", ddxq.KinhDo);
                    cmd.Parameters.AddWithValue("@KhoangCach", ddxq.KhoangCach);
                    cmd.Parameters.AddWithValue("@ThoiGianDiChuyen", ddxq.ThoiGianDiChuyen);
                    cmd.Parameters.AddWithValue("@ThoiGianCapNhat", ddxq.ThoiGianCapNhat);

                    conn.Open();
                    var rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // Cập nhật DDXQ
        public bool UpdateDDXQ(DDXQ ddxq)
        {
            var connectionString = Properties.Resources.MySqlConnection;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    var query = @"
                        UPDATE DiaDiemXungQuanh
                        SET MaCN = @MaCN, TenDD = @TenDD, LoaiDD = @LoaiDD, DiaChi = @DiaChi,
                            DanhGia = @DanhGia, ViDo = @ViDo, KinhDo = @KinhDo,
                            KhoangCach = @KhoangCach, ThoiGianDiChuyen = @ThoiGianDiChuyen,
                            ThoiGianCapNhat = @ThoiGianCapNhat
                        WHERE MaDD = @MaDD";

                    var cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@MaDD", ddxq.MaDD);
                    cmd.Parameters.AddWithValue("@MaCN", ddxq.MaCN);
                    cmd.Parameters.AddWithValue("@TenDD", ddxq.TenDD);
                    cmd.Parameters.AddWithValue("@LoaiDD", ddxq.LoaiDD);
                    cmd.Parameters.AddWithValue("@DiaChi", ddxq.DiaChi);
                    cmd.Parameters.AddWithValue("@DanhGia", ddxq.DanhGia);
                    cmd.Parameters.AddWithValue("@ViDo", ddxq.ViDo);
                    cmd.Parameters.AddWithValue("@KinhDo", ddxq.KinhDo);
                    cmd.Parameters.AddWithValue("@KhoangCach", ddxq.KhoangCach);
                    cmd.Parameters.AddWithValue("@ThoiGianDiChuyen", ddxq.ThoiGianDiChuyen);
                    cmd.Parameters.AddWithValue("@ThoiGianCapNhat", ddxq.ThoiGianCapNhat);

                    conn.Open();
                    var rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // Xóa mềm (soft delete)
        public bool DeleteDDXQ(int maDD)
        {
            var connectionString = Properties.Resources.MySqlConnection;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    var query = "DELETE FROM DiaDiemXungQuanh WHERE MaDD = @MaDD"; // hoặc: UPDATE DDXQ SET IsDeleted = 1
                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaDD", maDD);


                    conn.Open();
                    var rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

