using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class PhieuThue_Custom: PhieuThue
    {
        private string tenKH;
        private string tenNV;

        public string TenKH { get => tenKH; set => tenKH = value; }
        public string TenNV { get => tenNV; set => tenNV = value; }
    }
}
