using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;

namespace BUS
{
	public class BUS_TaiKhoan
	{
		private static BUS_TaiKhoan instance;

		private BUS_TaiKhoan() { }

		private static BUS_TaiKhoan getInstance()
		{
			if (instance == null)
			{
				instance = new BUS_TaiKhoan();
			}
			return instance;
		}


	}
}
