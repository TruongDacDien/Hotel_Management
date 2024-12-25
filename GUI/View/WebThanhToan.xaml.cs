using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;

namespace GUI.View
{
	/// <summary>
	/// Interaction logic for WebThanhToan.xaml
	/// </summary>
	public partial class WebThanhToan : Window
	{
		private string Url;
		private Action<string> onStatusChanged; // Để truyền dữ liệu trạng thái về cửa sổ chính

		public WebThanhToan(string url, Action<string> onStatusChanged)
		{
			InitializeComponent();
			Url = url;
			this.onStatusChanged = onStatusChanged;
		}

		private void WebThanhToans_Loaded(object sender, RoutedEventArgs e)
		{
			InitBrowser(Url);
		}

		private async Task initizated()
		{
			await webView.EnsureCoreWebView2Async(null); // Đảm bảo WebView2 đã sẵn sàng
		}

		private async void InitBrowser(string url)
		{
			await initizated(); // Đảm bảo WebView2 được khởi tạo
			webView.CoreWebView2.Navigate(url); // Điều hướng đến URL

			// Đăng ký sự kiện NavigationCompleted để kiểm tra URL khi điều hướng hoàn tất
			webView.CoreWebView2.NavigationCompleted += WebView2_NavigationCompleted;
		}

		private void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
		{
			var currentUrl = webView.CoreWebView2.Source;

			if (currentUrl.Contains("status=CANCELLED"))
			{
				// Trả lại trạng thái hủy thanh toán về cửa sổ chính
				onStatusChanged?.Invoke("CANCELLED");
				this.Close();
			}
			else if (currentUrl.Contains("status=PAID"))
			{
				// Trả lại trạng thái đã thanh toán về cửa sổ chính
				onStatusChanged?.Invoke("PAID");
				this.Close();
			}
		}
	}
}
