using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
	public class TienNghi
	{
		private int maTN;
		private string tenTN;

		public int MaTN { get => maTN; set => maTN = value; }
		public string TenTN { get => tenTN; set => tenTN = value; }
	}
}
