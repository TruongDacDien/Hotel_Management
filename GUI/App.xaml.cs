using BUS;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GUI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
	}
	public static class SettingsHelper
	{
		public static void UpdateSettings(string cancelUrl, string returnUrl)
		{
			Properties.Settings.Default.cancelUrl = cancelUrl;
			Properties.Settings.Default.returnUrl = returnUrl;
			Properties.Settings.Default.Save(); // Lưu các thay đổi vào file lưu trữ
		}
	}
}
