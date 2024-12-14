using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class ThongKe
    {
        private decimal doanhthuPhong;
        private decimal doanhthuDichVu;
        private decimal tongDoanhThu;
        private int sophong;

        public decimal DoanhthuPhong { get => doanhthuPhong; set => doanhthuPhong = value; }
        public decimal DoanhthuDichVu { get => doanhthuDichVu; set => doanhthuDichVu = value; }
        public decimal TongDoanhThu { get => tongDoanhThu; set => tongDoanhThu = value; }
        public int Sophong { get => sophong; set => sophong = value; }
    }
}
