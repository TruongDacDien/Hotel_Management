﻿using System.Collections.Generic;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
	public class PhieuThueBUS
	{
		private static PhieuThueBUS Instance;

		private PhieuThueBUS()
		{
		}

		public static PhieuThueBUS GetInstance()
		{
			if (Instance == null) Instance = new PhieuThueBUS();
			return Instance;
		}

		public bool addPhieuThue(PhieuThue pt, out string error)
		{
			return PhieuThueDAL.GetInstance().addPhieuThue(pt, out error);
		}

		public List<PhieuThue_Custom> getDataPhieuThue()
		{
			return PhieuThueDAL.GetInstance().getDataFromDB();
		}

		public bool xoaPhieuThueTheoMaPhieuThue(int maPhieuThue, out string error)
		{
			return PhieuThueDAL.GetInstance().xoaPhieuThueTheoMaPhieuThue(maPhieuThue, out error);
		}

		public int layMaPhieuThueMoiNhat()
		{
			return PhieuThueDAL.GetInstance().layMaPhieuThueMoiNhat();
		}
	}
}