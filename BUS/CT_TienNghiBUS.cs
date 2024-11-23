using DAL.Data;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class CT_TienNghiBUS
    {
        private static CT_TienNghiBUS instance;

        public static CT_TienNghiBUS Instance
        {
            get
            {
                if (instance == null) instance = new CT_TienNghiBUS();
                return instance;
            }
        }

        private CT_TienNghiBUS() { }

        public List<CT_TienNghi> getData()
        {
            return CT_TienNghiDAL.Instance.getData();
        }
        public bool addCTTienNghi(CT_TienNghi chiTietTN)
        {
            return CT_TienNghiDAL.Instance.addCTTienNghi(chiTietTN);
        }

        public bool xoaCTTienNghi(CT_TienNghi chiTietTN)
        {
            return CT_TienNghiDAL.Instance.xoaCTTienNghi(chiTietTN);
        }

        public bool capNhatCTTienNghi(CT_TienNghi chiTietTN)
        {
            return CT_TienNghiDAL.Instance.capnhatCTTienNghi(chiTietTN);
        }
        public bool KiemTraTonTai(CT_TienNghi chiTietTN)
        {
            return CT_TienNghiDAL.Instance.KiemTraTonTai(chiTietTN);
        }
    }
}
