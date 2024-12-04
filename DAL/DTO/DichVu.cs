using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class DichVu
	{
        private string tenDV;
        private string loaiDV;
        private int maLoaiDV;
        private decimal gia;
        private int maDV;

        public string TenDV { get => tenDV; set => tenDV = value; }
        public string LoaiDV { get => loaiDV; set => loaiDV = value; }
        public decimal Gia { get => gia; set => gia = value; }
        public int MaDV { get => maDV; set => maDV = value; }
        public int MaLoaiDV { get => maLoaiDV; set => maLoaiDV = value; }
    }
}
