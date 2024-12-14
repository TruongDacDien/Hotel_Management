using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL;
using DAL.DTO;


namespace BUS
{
    public class ThongKeBUS
    {
        private static ThongKeBUS Instance;
        private  ThongKeBUS()
        {

        }
        public static ThongKeBUS GetInstance()
        {
            if (Instance == null) 
            {
                Instance = new ThongKeBUS();
            }
            return Instance;
        }

        public ThongKe LoadThongKeTheoThangNam(int month, int year)
        {
           return ThongKe_DAL.GetInstance().LoadThongKeTheoThangNam(month, year);
        }
        public List<CTHD> LoadThongKeTheoNam(int year)
        {
            return ThongKe_DAL.GetInstance().LoadThongKeTheoNam(year);
        }
    }
  
}
