using BUS;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for Them_SuaChiTietTienNghi.xaml
    /// </summary>
    public partial class Them_SuaChiTietTienNghi : Window
    {
        public delegate void truyenData(CT_TienNghi CTTienNghi);
        public delegate void suaData(CT_TienNghi CTTienNghi);


        public truyenData truyenCT;
        public suaData suaCT;
        List<TienNghi> TienNghis;
        private string maCT;
        private bool isEditing;
        public Them_SuaChiTietTienNghi()
        {
            InitializeComponent();
            
            TaiDanhSach();
        }

        public Them_SuaChiTietTienNghi(bool isEditing = false, CT_TienNghi ct = null) : this()
        {
            this.isEditing = isEditing;

            cmbSoPhong.DisplayMemberPath = "SoPhong";
            cmbSoPhong.SelectedValuePath = "SoPhong";

            cmbTienNghi.DisplayMemberPath = "TenTN";
            cmbTienNghi.SelectedValuePath = "MaTN";
            cmbTienNghi.IsReadOnly = true;


            if (isEditing && ct != null) // Nếu là sửa, gán dữ liệu hiện tại vào UI
            {
                cmbSoPhong.Text = ct.SoPhong;
                cmbTienNghi.Text = ct.TenTN;
                txtSoLuong.Text = ct.SL.ToString();
                txbTitle.Text = "Sửa thông tin " + ct.MaCTTN;
                maCT = ct.MaCTTN.ToString();
            }
            else
            {
                txbTitle.Text = "Nhập thông tin chi tiết tiện nghi"; // Nếu là thêm mới, tiêu đề khác đi
			}


        }
        private void TaiDanhSach()
        {
            TienNghis = new List<TienNghi>(TienNghiBUS.Instance.getDataTienNghi());
            cmbSoPhong.ItemsSource = PhongBUS.GetInstance().getDataPhong();
            cmbTienNghi.ItemsSource = TienNghis;
            cmbSoPhong.DisplayMemberPath = "SoPhong";
            cmbSoPhong.SelectedValuePath = "SoPhong";
            cmbTienNghi.DisplayMemberPath = "TenTN";
            cmbTienNghi.SelectedValuePath = "MaTN";
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (!KiemTra())
            {
                return;
            }
            else
            {
                CT_TienNghi CTTienNghi = new CT_TienNghi()
                {
                    SoPhong = cmbSoPhong.SelectedValue.ToString(),
                    SL = int.Parse(txtSoLuong.Text),
                    TenTN = cmbTienNghi.Text,
                    MaTN = (int)cmbTienNghi.SelectedValue,
                };
                if (truyenCT != null)
                {
                    truyenCT(CTTienNghi);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
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
                    

                    CT_TienNghi ctTienNghi = new CT_TienNghi()
                    {

                        MaCTTN = string.IsNullOrEmpty(maCT) ? (int?)null : int.Parse(maCT),
                        SoPhong = cmbSoPhong.SelectedValue.ToString(),
                        SL = int.Parse(txtSoLuong.Text),
                        TenTN = cmbTienNghi.Text,
                        MaTN = (int)cmbTienNghi.SelectedValue,
                    };

                    if (suaCT != null)
                    {
                        suaCT(ctTienNghi);
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
        private bool KiemTra()
        {
            if (string.IsNullOrWhiteSpace(cmbSoPhong.Text))
            {
                new DialogCustoms("Vui lòng chọn số phòng","Thông báo",DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbTienNghi.Text))
            {
                new DialogCustoms("Vui lòng chọn tên tiện nghi", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                new DialogCustoms("Vui lòng nhập số lượng", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            else
            {
                int so;
                if (int.TryParse(txtSoLuong.Text, out so) == false)
                {
                    new DialogCustoms("Vui lòng nhập đúng định đạng số", "Thông báo", DialogCustoms.OK).Show();
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
