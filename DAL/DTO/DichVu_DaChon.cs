using System.ComponentModel;

namespace DAL.DTO
{
	public class DichVu_DaChon : INotifyPropertyChanged
	{
		private decimal? soLuong;
		private decimal? thanhTien;

		public string TenDV { get; set; }

		public int? MaDV { get; set; }

		public decimal Gia { get; set; }

		public decimal? SoLuong
		{
			get => soLuong;
			set
			{
				soLuong = value;
				OnPropertyChanged("SoLuong");
			}
		}


		public decimal? ThanhTien
		{
			get => thanhTien;
			set
			{
				thanhTien = value;
				OnPropertyChanged("ThanhTien");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string newName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(newName));
		}
	}
}