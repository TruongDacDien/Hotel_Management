using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
	public class BaoCaoDoanhSo
	{
		public int MABCDS {  get; set; }
		public decimal DOANHTHUPHONG { get; set; }
		public decimal DOANHTHUDICHVU { get; set; }
		public decimal TONGDOANHTHU {  get; set; }
		public int SOLUONGPHONGDAT {  get; set; }
		public decimal TILEPHONG {  get; set; }	
		public decimal TILEDICHVU { get; set; }
		public DateTime NGAYLAPBAOCAO { get; set; }
		public int MACTDP {  get; set; }
		public int MAHD {  get; set; }
	}
}
