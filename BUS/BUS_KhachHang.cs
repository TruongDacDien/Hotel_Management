using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;

namespace BUS
{
	public class BUS_KhachHang
	{
		private static BUS_KhachHang instance;

		private BUS_KhachHang() { }

		private static BUS_KhachHang getInstance()
		{
			if (instance == null)
			{
				instance = new BUS_KhachHang();
			}
			return instance;
		}

		public List<KhachHang> GetKhachHangs()
		{
			return DAO_KhachHang.GetInstance().ListKhachHang();
		}

		// Thêm khách hàng
		public bool ThemKhachHang(KhachHang kh)
		{
			return DAO_KhachHang.GetInstance().ThemKhachHang(kh);
		}

		//kiểm tra khách hàng đã tồn tại chưa dựa vào CCCD nếu rồi thì trả ra KhachHang nếu chưa thì trả ra null
		public int kiemTraTonTaiKhachHang(string cccd)
		{
			KhachHang kh = DAO_KhachHang.GetInstance().KiemTraTonTaiQuaCCCD(cccd);
			if (kh != null)
			{
				return kh.MAKH;
			}
			else
			{
				return -1;
			}
		}

		public bool SuaKhachHang(KhachHang khachHang)
		{
			return DAO_KhachHang.GetInstance().SuaKhachHang(khachHang);
		}

		public bool XoaKhachHang(int maKH)
		{
			return DAO_KhachHang.GetInstance().XoaKhachHang(maKH);
		}
	}
}
