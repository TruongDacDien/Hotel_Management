using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GUI.View;

namespace GUI.UserControls
{
	/// <summary>
	///     Interaction logic for uc_Home.xaml
	/// </summary>
	public partial class uc_Home : UserControl
	{
		private string baseDir;
		private int index;

		public uc_Home()
		{
			InitializeComponent();
			//lấy ra đường dẫn tương đối
			baseDir = Environment.CurrentDirectory;

			//ImageBrush ENABLED_BACKGROUND = new ImageBrush(new BitmapImage(new Uri(baseDir + "\\Res\\Home0.png")));
			var ENABLED_BACKGROUND = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Res/Home0.png")));
			Background = ENABLED_BACKGROUND;

			var dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += dispatcherTimer_Tick;
			dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
			dispatcherTimer.Start();
		}

		#region method

		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				index++;
				if (index > 5)
					index = 0;
				var ENABLED_BACKGROUND =
					new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Res/Home" + index + ".png")));
				Background = ENABLED_BACKGROUND;
			}
			catch (Exception ex)
			{
				new DialogCustoms("Lỗi: T " + ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
			}
		}

		#endregion

		#region event

		private void right_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				index++;
				if (index > 5)
					index = 0;
				var ENABLED_BACKGROUND =
					new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Res/Home" + index + ".png")));
				Background = ENABLED_BACKGROUND;
			}
			catch (Exception ex)
			{
				new DialogCustoms("Lỗi: R " + ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
			}
		}


		private void left_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				index--;
				if (index < 0)
					index = 5;
				var ENABLED_BACKGROUND =
					new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Res/Home" + index + ".png")));
				Background = ENABLED_BACKGROUND;
			}
			catch (Exception ex)
			{
				new DialogCustoms("Lỗi: L " + ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
			}
		}

		#endregion
	}
}