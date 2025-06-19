using System.ComponentModel;

namespace DAL.DTO
{
	public class TienNghi_DaChon : INotifyPropertyChanged
	{
		private decimal? soLuong;

		public string TenTN { get; set; }

		public int? MaTN { get; set; }

		public decimal? SoLuong
		{
			get => soLuong;
			set
			{
				soLuong = value;
				OnPropertyChanged("SoLuong");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string newName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(newName));
		}
	}
}