using System.ComponentModel;

namespace DAL.DTO
{
	public class TienNghi : INotifyPropertyChanged
	{
		private int _soLuong;
		private string _tenTN;
		private int _maTN;

		public int MaTN
		{
			get => _maTN;
			set
			{
				_maTN = value;
				OnPropertyChanged("MaTN");
			}
		}

		public string TenTN
		{
			get => _tenTN;
			set
			{
				_tenTN = value;
				OnPropertyChanged("TenTN");
			}
		}

		public int SoLuong
		{
			get => _soLuong;
			set
			{
				_soLuong = value;
				OnPropertyChanged("SoLuong");
			}
		}

		public int SoLuongBanDau { get; set; }

		public bool IsDeleted { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string newName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(newName));
		}
	}
}
