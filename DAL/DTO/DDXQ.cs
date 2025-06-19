using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class DDXQ
    {
        public int MaDD { get; set; }
        public int MaCN { get; set; }
        public string TenDD { get; set; }
        public string LoaiDD { get; set; }
        public string DiaChi { get; set; }
        public double DanhGia { get; set; }
        public double ViDo { get; set; }
        public double KinhDo { get; set; }
        public double KhoangCach { get; set; }
        public string ThoiGianDiChuyen { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }
    }
}
