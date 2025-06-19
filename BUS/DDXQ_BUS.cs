using DAL.Data;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class DDXQ_BUS
    {
        private static DDXQ_BUS Instance;

        private DDXQ_BUS() { }

        public static DDXQ_BUS GetInstance()
        {
            if (Instance == null) Instance = new DDXQ_BUS();
            return Instance;
        }

        public List<DDXQ> GetAllDDXQ()
        {
            return DDXQ_DAL.GetInstance().GetAllDDXQ();
        }

        public DDXQ GetDDXQByMaDD(int maDD)
        {
            return DDXQ_DAL.GetInstance().GetDDXQByMaDD(maDD);
        }

        // Thêm mới DDXQ
        public bool ThemDDXQ(DDXQ ddxq)
        {
            return DDXQ_DAL.GetInstance().AddDDXQ(ddxq);
        }

        // Cập nhật DDXQ
        public bool CapNhatDDXQ(DDXQ ddxq)
        {
            return DDXQ_DAL.GetInstance().UpdateDDXQ(ddxq);
        }

        // Xóa DDXQ
        public bool XoaDDXQ(int maDD)
        {
            return DDXQ_DAL.GetInstance().DeleteDDXQ(maDD);
        }
    }
}
