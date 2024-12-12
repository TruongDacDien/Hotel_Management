using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace BUS
{
    public class DichVuBUS
    {
        private static DichVuBUS Instance;

        private DichVuBUS()
        {

        }

        public static DichVuBUS GetInstance()
        {
            if (Instance == null)
            {
                Instance = new DichVuBUS();
            }
            return Instance;
        }

        public List<DichVu> getDichVu_Custom()
        {
            return DichVuDAL.GetInstance().getDataDichVu_Custom();
        }

        public List<DichVu> getDichVu()
        {
            return DichVuDAL.GetInstance().getDataDichVu();
        }
        public bool ThemDichVu(DichVu dv)
        {
            return DichVuDAL.GetInstance().addDichVu(dv);
        }
        public void xoaDataDichVu(DichVu dv)
        {
            DichVuDAL.GetInstance().xoaDichVu(dv);
        }

        public bool capNhatDichVu(DichVu dv)
        {
            return DichVuDAL.GetInstance().capNhatDichVu(dv);
        }

        public bool KiemTraTrungTen(DichVu dv)
        {
            return DichVuDAL.GetInstance().KiemTraTrungTen(dv);
        }

        public bool hienThiLaiDichVu(string tenDV)
        {
            return DichVuDAL.GetInstance().hienThiLaiDichVu(tenDV);
        }
    }
}
