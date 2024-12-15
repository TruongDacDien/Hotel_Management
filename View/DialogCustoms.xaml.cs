using System;
using System.Media;
using System.Windows;

namespace GUI.View
{
	/// <summary>
	///     Interaction logic for DialogCustoms.xaml
	/// </summary>
	public partial class DialogCustoms : Window
	{
		public static int YesNo = 1;
		public static int OK = 0;

		public DialogCustoms()
		{
			InitializeComponent();
			PlaySound(); // Phát âm thanh khi cửa sổ được khởi tạo
		}

		public DialogCustoms(string mess, string title, int mode) : this()
		{
			try
			{
				Title = title;
				txblMess.Text = mess;

				if (mode == YesNo)
				{
					btnOK.Visibility = Visibility.Hidden;
				}
				else
				{
					btnYes.Visibility = Visibility.Hidden;
					btnNo.Visibility = Visibility.Hidden;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi DialogCustom :" + ex.Message);
			}
		}

		private void btnNo_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void btnYes_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void PlaySound()
		{
			try
			{
				var resourceStream = Application
					.GetResourceStream(new Uri("pack://application:,,,/Res/notification_sound.wav"))?.Stream;
				// Tạo SoundPlayer với stream và phát âm thanh
				var soundPlayer = new SoundPlayer(resourceStream);
				soundPlayer.Play();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi phát âm thanh: " + ex.Message);
			}
		}
	}
}