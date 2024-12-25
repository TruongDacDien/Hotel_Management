using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.UserControls
{
	/// <summary>
	///     Interaction logic for ucControlbar2.xaml
	/// </summary>
	public partial class ucControlbar2 : UserControl
	{
		public ucControlbar2()
		{
			InitializeComponent();
		}

		private void Button_Close(object sender, RoutedEventArgs e)
		{
			var btn_close = sender as Button;
			var mainwindows = Window.GetWindow(btn_close);
			mainwindows.Close();
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
	}
}