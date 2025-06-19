using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BUS;
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

        private void TaiDanhSach()
        {
            allData = DDXQ_BUS.GetInstance().GetAllDDXQ();
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

        private void click_XoaDDXQ(object sender, RoutedEventArgs e)
        {
            var ddxq = (sender as Button).DataContext as DDXQ;
            var dlg = new DialogCustoms($"Bạn có muốn xóa địa điểm {ddxq.TenDD}?", "Thông báo", DialogCustoms.YesNo);
            if (dlg.ShowDialog() == true)
            {
                if (DDXQ_BUS.GetInstance().XoaDDXQ(ddxq.MaDD))
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

        private void NhanData(DDXQ ddxq)
        {
            if (DDXQ_BUS.GetInstance().ThemDDXQ(ddxq))
            {
                new DialogCustoms("Thêm địa điểm thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
                currentPage = 1;
                TaiDanhSach();
            }
        }

        private void SuaThongTinDDXQ(DDXQ ddxq)
        {
            if (DDXQ_BUS.GetInstance().CapNhatDDXQ(ddxq))
            {
                new DialogCustoms("Cập nhật địa điểm thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
                TaiDanhSach();
            }
        }

        #endregion
    }
}
