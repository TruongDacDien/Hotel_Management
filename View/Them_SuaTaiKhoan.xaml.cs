using BUS;
using DAL.Data;
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
		private bool isEditing;

        ObservableCollection<NhanVien> list;
        public Them_SuaTaiKhoan()
		{
			InitializeComponent();

            list = new ObservableCollection<NhanVien>(NhanVienBUS.GetInstance().getDataNhanVien());
            cmbMaNV.ItemsSource = list;
            cmbMaNV.DisplayMemberPath = "DisplayInfo";
			cmbMaNV.SelectedValuePath = "MaNV";
		}

		public Them_SuaTaiKhoan(bool isEditing = false, TaiKhoan taiKhoan = null) : this()
		{
			this.isEditing = isEditing;
			txtUsername.IsReadOnly = true;

			if (isEditing && taiKhoan != null)
			{
				txtUsername.Text = taiKhoan.Username;
				cmbCapDo.Text = taiKhoan.CapDoQuyen.ToString();
				cmbMaNV.Text = taiKhoan.NhanVien.DisplayInfo.ToString();
				txbTitle.Text = "Sửa thông tin tài khoản " + taiKhoan.Username;
			}
			else
			{
				txbTitle.Text = "Nhập thông tin tài khoản";
			}
		}

		#region Method
		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtUsername.Text) || txtUsername.Text.Any(ch => !char.IsLetterOrDigit(ch)))
			{
				new DialogCustoms("Vui lòng nhập tên tài khoản và không chứa ký tự đặc biệt", "Thông báo", DialogCustoms.OK).Show();
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
			return true;
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
				if (isEditing)
				{
					string pass = string.Empty;
					if (string.IsNullOrWhiteSpace(txtPassword.Text))
					{
						pass = TaiKhoanDAL.GetInstance().layTaiKhoanTheoUsername(txtUsername.Text).Password;
					}
					else
					{
						pass = Bcrypt_HashBUS.GetInstance().HashMatKhau(txtPassword.Text);
					}	
					TaiKhoan taiKhoan = new TaiKhoan()
					{
						Username = txtUsername.Text,
						Password = pass,
						CapDoQuyen = int.Parse(cmbCapDo.Text),
						MaNV = int.Parse(cmbMaNV.SelectedValue.ToString())
					};

					if (suaTaiKhoan != null)
					{
						suaTaiKhoan(taiKhoan);
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
				if (string.IsNullOrWhiteSpace(txtPassword.Text))
				{
					new DialogCustoms("Vui lòng nhập mật khẩu", "Thông báo", DialogCustoms.OK).Show();
					return;
				}
				string pass = Bcrypt_HashBUS.GetInstance().HashMatKhau(txtPassword.Text);
                TaiKhoan taiKhoan = new TaiKhoan()
				{
					Username = txtUsername.Text,
					Password = pass,
                    CapDoQuyen = int.Parse(cmbCapDo.Text),
                    MaNV = int.Parse(cmbMaNV.SelectedValue.ToString())
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
