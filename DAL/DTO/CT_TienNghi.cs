using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class CT_TienNghi
	{
		private int maCTTN;
		private int maTN;
		private string soPhong;
		private int sL;
		private string tenTN;

		public int MaCTTN { get => maCTTN; set => maCTTN = value; }
		public int MaTN { get => maTN; set => maTN = value; }
		public string SoPhong { get => soPhong; set => soPhong = value; }
		public int SL { get => sL; set => sL = value; }
		public string TenTN { get => tenTN; set => tenTN = value; }
	}
}
