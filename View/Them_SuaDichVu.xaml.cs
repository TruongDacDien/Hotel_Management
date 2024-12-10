using BUS;
using DAL.DTO;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for Them_SuaDichVu.xaml
    /// </summary>
    public partial class Them_SuaDichVu : Window
    {
        public delegate void TryenDuLieu(DichVuDTO dv);
        public delegate void SuaDuLieu(DichVuDTO dv);

        public TryenDuLieu truyen;
        public SuaDuLieu sua;
        private List<DichVuDTO> loaiDV;
        private string maDV;
        private bool isEditing;
        public Them_SuaDichVu()
        {
            InitializeComponent();
            TaiDanhSach();
        }

        private void TaiDanhSach()
        {
            loaiDV = new List<DichVuDTO>(DichVuBUS.GetInstance().getLoaiDichVu());
            Console.WriteLine($"Số lượng loại DV: {loaiDV.Count}");
           
            cmbMaLoai.ItemsSource = loaiDV;
            cmbMaLoai.DisplayMemberPath = "LoaiDV";
            cmbMaLoai.SelectedValuePath = "MaLoaiDV";
        }
        public Them_SuaDichVu(bool isEditing = false, DichVuDTO dv = null) : this()
        {
            this.isEditing = isEditing;
            txtTenDichVu.IsReadOnly = false;
            cmbMaLoai.DisplayMemberPath = "LoaiDV";
            cmbMaLoai.SelectedValuePath = "MaLoaiDV";

            if (isEditing && dv != null)
            {
                txtTenDichVu.Text = dv.TenDV;
                cmbMaLoai.Text = dv.LoaiDV;
                txtGia.Text = dv.Gia % 1 == 0 ? ((int)dv.Gia).ToString() : dv.Gia.ToString();
                //txtGia.Text = dv.Gia.ToString();
                txbTitle.Text = "Sửa thông tin dịch vụ " + dv.MaDV;
                maDV = dv.MaDV.ToString();
            }
            else
            {
                txbTitle.Text = "Nhập thông tin dịch vụ";
            }
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            if (!KiemTra())
            {
                return;
            }
            else
            {
                if (isEditing)
                {                
                    DichVuDTO dichVu = new DichVuDTO()
                    {
                        MaDV = int.Parse(maDV.ToString()),
                        TenDV = txtTenDichVu.Text,
                        Gia = int.Parse(txtGia.Text),
                        MaLoaiDV = (int)cmbMaLoai.SelectedValue,
                    };
                    if (sua != null)
                    {
                        sua(dichVu);
                    }
                }
                else
                {
                    btnThem_Click(sender, e);
                }    
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {

            if (!KiemTra())
            {
                return;
            }
            else
            {
                DichVuDTO dichVu = new DichVuDTO()
                {
                    TenDV = txtTenDichVu.Text,
                    Gia = int.Parse(txtGia.Text),
                    MaLoaiDV = (int)cmbMaLoai.SelectedValue,
                };
                if (truyen != null)
                {
                    truyen(dichVu);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }

        private bool KiemTra()
        {
            if (string.IsNullOrWhiteSpace(txtTenDichVu.Text))
            {
                new DialogCustoms("Vui lòng nhập tên dịch vụ","Thông báo",DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbMaLoai.Text))
            {
                new DialogCustoms("Vui lòng chọn mã loại dịch vụ", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtGia.Text))
            {
                new DialogCustoms("Vui lòng nhập giá", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            else
            {
                int so;
                if (int.TryParse(txtTenDichVu.Text, out so) == true)
                {
                    new DialogCustoms("Vui lòng nhập đúng định đạng tên dịch vụ", "Thông báo", DialogCustoms.OK).Show();
                    return false;
                }
                if (int.TryParse(txtGia.Text, out so) == false)
                {
                    new DialogCustoms("Vui lòng nhập đúng định đạng giá", "Thông báo", DialogCustoms.OK).Show();
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
