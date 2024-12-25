using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GUI.View;

namespace GUI.UserControls
{
	/// <summary>
	///     Interaction logic for uc_Controlbar.xaml
	/// </summary>
	public partial class uc_Controlbar : UserControl
	{
		public uc_Controlbar()
		{
			InitializeComponent();
		}

		#region event

		private void Button_Close(object sender, RoutedEventArgs e)
		{
			var dialog = new DialogCustoms("Bạn có muốn thoát ứng dụng?", "Thông báo", DialogCustoms.YesNo);
			if (dialog.ShowDialog() == true)
			{
				var btn_close = sender as Button;
				var mainwindows = Window.GetWindow(btn_close);
				mainwindows.Close();
			}
		}

		private void Button_Maximize(object sender, RoutedEventArgs e)
		{
			var btn_close = sender as Button;
			var mainwindows = Window.GetWindow(btn_close);
			if (mainwindows != null)
			{
				if (mainwindows.WindowState != WindowState.Maximized)
				{
					mainwindows.WindowState = WindowState.Maximized;
					btn_maximize.ToolTip = "Normal";
					controlbar.MinHeight = 1;
				}
				else
				{
					mainwindows.WindowState = WindowState.Normal;
					btn_maximize.ToolTip = "Maximize";
					controlbar.MinHeight = 0;
				}

				mainwindows.UpdateLayout();
			}
		}

		private void Button_Minimize(object sender, RoutedEventArgs e)
		{
			var btn_close = sender as Button;
			var mainwindows = Window.GetWindow(btn_close);
			mainwindows.WindowState = WindowState.Minimized;
		}

		private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var grid = sender as Grid;
			var mainwindows = Window.GetWindow(grid);
			mainwindows.DragMove();
		}

		#endregion
	}
}