using BUS;
using DAL.DTO;
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
using System.Windows.Shapes;

namespace GUI.View
{
	/// <summary>
	/// Interaction logic for Them_SuaTaiKhoan.xaml
	/// </summary>
	public partial class Them_SuaTaiKhoan : Window
	{
        public delegate void truyenData(TaiKhoan taiKhoan);
        public delegate void suaData(TaiKhoan taiKhoan);


        public truyenData truyenTaiKhoan;
        public suaData suaTaiKhoan;

        private string username;
        ObservableCollection<NhanVien> list;
        public Them_SuaTaiKhoan()
		{
			InitializeComponent();

            list = new ObservableCollection<NhanVien>(NhanVienBUS.GetInstance().getDataNhanVien());
            cmbMaNV.ItemsSource = list;
            cmbMaNV.DisplayMemberPath = "MaNV";
            cmbMaNV.SelectedValuePath = "MaNV";

        }

		public Them_SuaTaiKhoan(TaiKhoan taiKhoan) : this()
		{
			txtUsername.IsReadOnly = true;

			txbTitle.Text = "Sửa thông tin " + taiKhoan.Username;
			txtUsername.Text = taiKhoan.Username;
			
			cmbCapDo.Text = taiKhoan.CapDoQuyen.ToString();
			cmbMaNV.Text = taiKhoan.MaNV.ToString();
		}

		#region Method
		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtUsername.Text))
			{
				new DialogCustoms("Vui lòng nhập tên tài khoản", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (string.IsNullOrWhiteSpace(cmbCapDo.Text))
			{
				new DialogCustoms("Vui lòng nhập cấp độ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}
			if (string.IsNullOrWhiteSpace(cmbMaNV.Text))
			{
				new DialogCustoms("Vui lòng chọn mã nhân viên", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}
			else
			{
				//int so;
                if (!txtUsername.Text.Any(char.IsLetter) || txtUsername.Text.All(char.IsDigit))
                {
                    new DialogCustoms("Vui lòng nhập đúng định dạng tên loại phòng (phải chứa ít nhất một chữ cái và không được hoàn toàn là số)",
                                      "Thông báo", DialogCustoms.OK).Show();
                    return false;
                }
                if (txtUsername.Text.Any(ch => !char.IsLetterOrDigit(ch)))
                {
                    new DialogCustoms("Tên loại phòng không được chứa ký tự đặc biệt", "Thông báo", DialogCustoms.OK).Show();
                    return false;
                }
                //if (int.TryParse(txtPassword.Text, out so) == false)
                //{
                //	new DialogCustoms("Vui lòng nhập đúng định dạng số người tối đa", "Thông báo", DialogCustoms.OK).Show();
                //	return false;
                //}
                else
				{
					return true;
				}
			}
		}
		#endregion

		#region Event
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
				
				TaiKhoan taiKhoan;
				if (txtPassword.Text == "")
				{
                    Console.WriteLine("Rỗng: " +txtPassword.Text);
                   
                   taiKhoan = new TaiKhoan()
                    {
                        Username = txtUsername.Text,
                       
                        CapDoQuyen = int.Parse(cmbCapDo.Text),
                        MaNV = int.Parse(cmbMaNV.Text),
                    };
                }
				else
				{
                    string pass = Bcrypt_HashBUS.GetInstance().HashMatKhau(txtPassword.Text);
                    taiKhoan = new TaiKhoan()
                    {
                        Username = txtUsername.Text,
                        Password = pass,
                        CapDoQuyen = int.Parse(cmbCapDo.Text),
                        MaNV = int.Parse(cmbMaNV.Text),
                    };
                }
				
				if (suaTaiKhoan != null)
				{
					suaTaiKhoan(taiKhoan);
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
				if (txtPassword.Text == null)
				{
                    new DialogCustoms("Vui lòng nhập mật khẩu", "Thông báo", DialogCustoms.OK).Show();
                    return ;
                }	
                string pass = Bcrypt_HashBUS.GetInstance().HashMatKhau(txtPassword.Text);
                TaiKhoan taiKhoan = new TaiKhoan()
				{
					Username = txtUsername.Text,
					Password = pass,
                    CapDoQuyen = int.Parse(cmbCapDo.Text),
                    MaNV = int.Parse(cmbMaNV.Text),
				};
                if (truyenTaiKhoan != null)
				{
					truyenTaiKhoan(taiKhoan);
				}
			}
			Window wd = Window.GetWindow(sender as Button);
			wd.Close();
		}
		#endregion
	}
}
