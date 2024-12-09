﻿using BUS;
using DAL.DTO;
using GUI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.UserControls
{
    /// <summary>
    /// Interaction logic for uc_QuanLyChiTietTienNghi.xaml
    /// </summary>
    public partial class uc_QuanLyChiTietTienNghi : UserControl
    {
        ObservableCollection<CT_TienNghi> list;
        private CollectionView view;

        public uc_QuanLyChiTietTienNghi()
        {
            InitializeComponent();
            TaiDanhSach();
            
        }

        private void TaiDanhSach()
        {
            list = new ObservableCollection<CT_TienNghi>(CT_TienNghiBUS.Instance.getData());
            lsvCTTienNghi.ItemsSource = list;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lsvCTTienNghi.ItemsSource);
            view.Filter = CTTienNghiFilter;
        }


        private bool CTTienNghiFilter(object obj)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return (obj as CT_TienNghi).SoPhong.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void btnThemCTTienNghi_Click(object sender, RoutedEventArgs e)
        {
            Them_SuaChiTietTienNghi ThemCTTienNghi = new Them_SuaChiTietTienNghi(false);
            ThemCTTienNghi.truyenCT = new Them_SuaChiTietTienNghi.truyenData(nhanData);
            ThemCTTienNghi.ShowDialog();
        }

        private void btnSuaCTTienNghi_Click(object sender, RoutedEventArgs e)
        {
            CT_TienNghi ctTienNghi = (sender as Button).DataContext as CT_TienNghi;
            Them_SuaChiTietTienNghi CapNhatCTTienNghi = new Them_SuaChiTietTienNghi(true, ctTienNghi);
            CapNhatCTTienNghi.suaCT = new Them_SuaChiTietTienNghi.suaData(capNhatData);
            CapNhatCTTienNghi.ShowDialog();
        }

        private void btnXoaCTTienNghi_Click(object sender, RoutedEventArgs e)
        {
            CT_TienNghi cT_TienNghi = (sender as Button).DataContext as CT_TienNghi;

            var ThongBao = new DialogCustoms("Bạn có thật sự muốn xóa tiện nghi " + cT_TienNghi.TenTN + " ở phòng "+ cT_TienNghi.SoPhong, "Thông báo", DialogCustoms.YesNo);

            if (ThongBao.ShowDialog() == true)
            {
                new DialogCustoms("Xóa thành công thành công", "Thông báo", DialogCustoms.OK).ShowDialog();
                CT_TienNghiBUS.Instance.xoaCTTienNghi(cT_TienNghi);
                TaiDanhSach();
            }
        }

        void nhanData(CT_TienNghi cT_TienNghi)
        {
            if (!CT_TienNghiBUS.Instance.KiemTraTonTai(cT_TienNghi))
            {
                if (CT_TienNghiBUS.Instance.addCTTienNghi(cT_TienNghi))
                {
                    new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
                    TaiDanhSach();
                }
            }
            else
            {
                if(CT_TienNghiBUS.Instance.hienThiLaiCT_TienNghi(cT_TienNghi.MaTN, cT_TienNghi.SoPhong))
                {
					new DialogCustoms("Tiện nghi đã tồn tại trong hệ thống\nĐã cập nhật lại danh sách", "Thông báo", DialogCustoms.OK).Show();
                    TaiDanhSach();
				}
			}
           
        }
        void capNhatData(CT_TienNghi cT_TienNghi)
        {
            if (CT_TienNghiBUS.Instance.capNhatCTTienNghi(cT_TienNghi))
            {
                new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
                TaiDanhSach();
            }

            else
            {
                new DialogCustoms("Tiện nghi đã tồn tại", "Thông báo", DialogCustoms.OK).Show();
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lsvCTTienNghi.ItemsSource).Refresh();
        }
    }
}
