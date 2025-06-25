using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BUS;
using DAL.Data;
using DAL.DTO;
using GUI.View;

namespace GUI.UserControls
{
	public partial class uc_DDXQ : UserControl
	{
		private List<DDXQ> allData;                    // Toàn bộ dữ liệu
		private ObservableCollection<DDXQ> pageData;   // Dữ liệu đang hiển thị
		private int currentPage = 1;
		private int pageSize = 10;
		private int totalPages = 0;

		public uc_DDXQ()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		#region Method

		private async void TaiDanhSach()
		{
			allData = await DDXQ_BUS.GetInstance().GetAllDDXQ();
			totalPages = (int)Math.Ceiling((double)allData.Count / pageSize);
			pageData = new ObservableCollection<DDXQ>();
			lvDDXQ.ItemsSource = pageData;

			LoadCurrentPage();
		}

		private void LoadCurrentPage()
		{
			pageData.Clear();

			var filtered = allData;

			// Nếu có tìm kiếm
			if (!string.IsNullOrWhiteSpace(txbTimKiem.Text))
			{
				string keyword = RemoveVietnameseTone(txbTimKiem.Text.Trim());
				filtered = allData.Where(x =>
					RemoveVietnameseTone(x.TenDD).Contains(keyword)
				).ToList();
			}

			totalPages = (int)Math.Ceiling((double)filtered.Count / pageSize);
			currentPage = Math.Max(1, Math.Min(currentPage, totalPages)); // tránh out of bound

			var items = filtered
						.Skip((currentPage - 1) * pageSize)
						.Take(pageSize)
						.ToList();

			foreach (var item in items)
				pageData.Add(item);
		}

		private string RemoveVietnameseTone(string text)
		{
			var result = text.ToLower();
			result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ", "a");
			result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ", "e");
			result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ", "i");
			result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ", "o");
			result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ", "u");
			result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ", "y");
			result = Regex.Replace(result, "đ", "d");
			return result;
		}

		#endregion

		#region Event

		private void TimKiem_TextChanged(object sender, TextChangedEventArgs e)
		{
			currentPage = 1;
			LoadCurrentPage();
		}

		private async void click_CapNhatDDXQ(object sender, RoutedEventArgs e)
		{
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "restaurant", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "hotel", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "cafe", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "bar", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "amusement_park", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "aquarium", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "bowling_alley", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "casino", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "movie_theater", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "museum", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "night_club", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "park", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "tourist_attraction", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "zoo", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "lodging", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "rv_park", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "campground", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "gym", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "stadium", 7);
			await DDXQ_BUS.GetInstance().FetchAndSaveNearbyLocationsAsync(1, 15, "spa", 7);

			new DialogCustoms("Cập nhật địa điểm thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
			TaiDanhSach(); // reload lại
		}

		private void click_ThemDDXQ(object sender, RoutedEventArgs e)
		{
			var td = new Them_SuaDDXQ();
			td.themDDXQ = NhanData;
			td.ShowDialog();
		}

		private void click_SuaDDXQ(object sender, RoutedEventArgs e)
		{
			var ddxq = (sender as Button).DataContext as DDXQ;
			var suaDDXQ = new Them_SuaDDXQ();
			suaDDXQ.truyenDDXQ(ddxq);
			suaDDXQ.suaDDXQ = SuaThongTinDDXQ;
			suaDDXQ.ShowDialog();
		}

		private async Task click_XoaDDXQ(object sender, RoutedEventArgs e)
		{
			var ddxq = (sender as Button).DataContext as DDXQ;
			var dlg = new DialogCustoms($"Bạn có muốn xóa địa điểm {ddxq.TenDD}?", "Thông báo", DialogCustoms.YesNo);
			if (dlg.ShowDialog() == true)
			{
				if (await DDXQ_BUS.GetInstance().XoaDDXQ(ddxq.MaDD))
				{
					new DialogCustoms("Xóa địa điểm thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
					currentPage = 1;
					TaiDanhSach();
				}
			}
		}

		private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
		{
			if (currentPage > 1)
			{
				currentPage--;
				LoadCurrentPage();
			}
		}

		private void BtnNextPage_Click(object sender, RoutedEventArgs e)
		{
			if (currentPage < totalPages)
			{
				currentPage++;
				LoadCurrentPage();
			}
		}

		private async void NhanData(DDXQ ddxq)
		{
			if ((await DDXQ_BUS.GetInstance().ThemDDXQ(ddxq)) > 0) // thêm await
			{
				new DialogCustoms("Thêm địa điểm thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
				currentPage = 1;
				TaiDanhSach(); // gọi async void, được phép
			}
		}

		private async void SuaThongTinDDXQ(DDXQ ddxq)
		{
			if (await DDXQ_BUS.GetInstance().CapNhatDDXQ(ddxq.MaDD, ddxq)) // thêm await và truyền đủ tham số
			{
				new DialogCustoms("Cập nhật địa điểm thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
				TaiDanhSach(); // gọi async void
			}
		}

		#endregion
	}
}
