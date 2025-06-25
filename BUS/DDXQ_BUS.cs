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

        public async Task<List<DDXQ>> GetAllDDXQ()
        {
            return await DDXQ_DAL.GetInstance().GetAllAsync();
        }

        public async Task<DDXQ> GetDDXQByMaDD(int maDD)
        {
            return await DDXQ_DAL.GetInstance().GetByIdAsync(maDD);
        }

        // Thêm mới DDXQ
        public async Task<int> ThemDDXQ(DDXQ ddxq)
        {
            return await DDXQ_DAL.GetInstance().CreateAsync(ddxq);
        }

        // Cập nhật DDXQ
        public async Task<bool> CapNhatDDXQ(int id, DDXQ ddxq)
        {
            return await DDXQ_DAL.GetInstance().UpdateAsync(id, ddxq);
        }

        // Xóa DDXQ
        public async Task<bool> XoaDDXQ(int maDD)
        {
            return await DDXQ_DAL.GetInstance().DeleteAsync(maDD);
        }

		public async Task<string> FetchAndSaveNearbyLocationsAsync(int branchId, int radius, string type, int limit)
        {
            return await DDXQ_DAL.GetInstance().FetchAndSaveNearbyLocationsAsync(branchId, radius, type, limit);
        }
	}
}
