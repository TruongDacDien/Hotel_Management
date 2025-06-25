using System;
using System.Windows;
using DAL.DTO;

namespace GUI.View
{
    public partial class Them_SuaDDXQ : Window
    {
        public Action<DDXQ> themDDXQ;
        public Action<DDXQ> suaDDXQ;

        private bool isEdit = false;
        private int maDD;

        public Them_SuaDDXQ()
        {
            InitializeComponent();
        }

        public void truyenDDXQ(DDXQ ddxq)
        {
            isEdit = true;
            maDD = ddxq.MaDD;

            txbTenDD.Text = ddxq.TenDD;
            txbLoaiDD.Text = ddxq.LoaiDD;
            txbDiaChi.Text = ddxq.DiaChi;
            txbDanhGia.Text = ddxq.DanhGia.ToString();
            txbViDo.Text = ddxq.ViDo.ToString();
            txbKinhDo.Text = ddxq.KinhDo.ToString();
            txbKhoangCach.Text = ddxq.KhoangCach.ToString();
            txbThoiGianDiChuyen.Text = ddxq.ThoiGianDiChuyen;
            dtThoiGianCapNhat.SelectedDate = ddxq.ThoiGianCapNhat;
            txbMaCN.Text = ddxq.MaCN.ToString();
        }

        private void click_Luu(object sender, RoutedEventArgs e)
        {
            try
            {
                var ddxq = new DDXQ
                {
                    MaDD = maDD,
                    TenDD = txbTenDD.Text.Trim(),
                    LoaiDD = txbLoaiDD.Text.Trim(),
                    DiaChi = txbDiaChi.Text.Trim(),
                    DanhGia = decimal.TryParse(txbDanhGia.Text.Trim(), out var dg) ? dg : 0,
                    ViDo = decimal.TryParse(txbViDo.Text.Trim(), out var vido) ? vido : 0,
                    KinhDo = decimal.TryParse(txbKinhDo.Text.Trim(), out var kinhdo) ? kinhdo : 0,
                    KhoangCach = decimal.TryParse(txbKhoangCach.Text.Trim(), out var kc) ? kc : 0,
                    ThoiGianDiChuyen = txbThoiGianDiChuyen.Text.Trim(),
                    ThoiGianCapNhat = dtThoiGianCapNhat.SelectedDate ?? DateTime.Now,
                    MaCN = int.TryParse(txbMaCN.Text.Trim(), out var macn) ? macn : 0
                };

                if (isEdit)
                {
                    suaDDXQ?.Invoke(ddxq);
                }
                else
                {
                    themDDXQ?.Invoke(ddxq);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhập dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void click_Huy(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
