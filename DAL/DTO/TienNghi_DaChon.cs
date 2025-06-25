using System.ComponentModel;

namespace DAL.DTO
{
	public class TienNghi_DaChon : INotifyPropertyChanged
	{
		private int soLuong;

		public string TenTN { get; set; }

		public int MaTN { get; set; }

		public int SoLuong
		{
			get => soLuong;
			set
			{
				soLuong = value;
				OnPropertyChanged("SoLuong");
			}
		}

		public int SoLuongTruKhoLucThem { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string newName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(newName));
		}
	}
}