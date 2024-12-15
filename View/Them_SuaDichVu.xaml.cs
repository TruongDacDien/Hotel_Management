using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BUS;
using DAL.DTO;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for Them_SuaDichVu.xaml
	/// </summary>
	public partial class Them_SuaDichVu : Window
	{
		public delegate void SuaDuLieu(DichVu dv);

		public delegate void TryenDuLieu(DichVu dv);

		private readonly bool isEditing;
		private readonly string maDV;
		private List<LoaiDV> loaiDV;
		public SuaDuLieu sua;

		public TryenDuLieu truyen;

		public Them_SuaDichVu()
		{
			InitializeComponent();
			TaiDanhSach();
		}

		public Them_SuaDichVu(bool isEditing = false, DichVu dv = null) : this()
		{
			this.isEditing = isEditing;
			txtTenDichVu.IsReadOnly = false;
			cmbMaLoai.DisplayMemberPath = "TenLoaiDV";
			cmbMaLoai.SelectedValuePath = "MaLoaiDV";

			if (isEditing && dv != null)
			{
				txtTenDichVu.Text = dv.TenDV;
				cmbMaLoai.Text = dv.TenLoaiDV;
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

		private void TaiDanhSach()
		{
			loaiDV = new List<LoaiDV>(LoaiDichVuBUS.Instance.getDataLoaiDV());
			Console.WriteLine($"Số lượng loại DV: {loaiDV.Count}");

			cmbMaLoai.ItemsSource = loaiDV;
			cmbMaLoai.DisplayMemberPath = "TenLoaiDV";
			cmbMaLoai.SelectedValuePath = "MaLoaiDV";
		}

		private void btnHuy_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void btnCapNhat_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			if (isEditing)
			{
				var dichVu = new DichVu
				{
					MaDV = int.Parse(maDV),
					TenDV = txtTenDichVu.Text,
					Gia = int.Parse(txtGia.Text),
					MaLoaiDV = (int)cmbMaLoai.SelectedValue
				};
				if (sua != null) sua(dichVu);
			}
			else
			{
				btnThem_Click(sender, e);
			}

			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private void btnThem_Click(object sender, RoutedEventArgs e)
		{
			if (!KiemTra()) return;

			var dichVu = new DichVu
			{
				TenDV = txtTenDichVu.Text,
				Gia = int.Parse(txtGia.Text),
				MaLoaiDV = (int)cmbMaLoai.SelectedValue
			};
			if (truyen != null) truyen(dichVu);
			var wd = GetWindow(sender as Button);
			wd.Close();
		}

		private bool KiemTra()
		{
			if (string.IsNullOrWhiteSpace(txtTenDichVu.Text))
			{
				new DialogCustoms("Vui lòng nhập tên dịch vụ", "Thông báo", DialogCustoms.OK).Show();
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

			int so;
			if (int.TryParse(txtTenDichVu.Text, out so))
			{
				new DialogCustoms("Vui lòng nhập đúng định đạng tên dịch vụ", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			if (int.TryParse(txtGia.Text, out so) == false)
			{
				new DialogCustoms("Vui lòng nhập đúng định đạng giá", "Thông báo", DialogCustoms.OK).Show();
				return false;
			}

			return true;
		}
	}
}