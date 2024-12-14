using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class CTHD
    {
        private decimal tienPhong;
        private DateTime ngayLap;
        private decimal tongTien;
        private decimal tienDV;

        public decimal TienDV { get => tienDV; set => tienDV = value; }
        public decimal TienPhong { get => tienPhong; set => tienPhong = value; }
        public DateTime NgayLap { get => ngayLap; set => ngayLap = value; }
        public decimal TongTien { get => tongTien; set => tongTien = value; }

    }
}
