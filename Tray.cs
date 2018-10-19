using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Hardcodet.Wpf.TaskbarNotification;

namespace QuickerAccess {

	/// <summary>
	/// Class to manage tray icon and interaction with tray
	/// </summary>
	internal class Tray {

		private TaskbarIcon icon;

		/// <summary>
		/// Default constructor, 
		/// </summary>
		public Tray() {
			icon = new TaskbarIcon {
				IconSource = new BitmapImage(new Uri(AppHelper.resourcesPath + "TrayIcon.ico")),
				ContextMenu = new ContextMenu()
			};

			icon.TrayLeftMouseDown += App.mapper.Tray;

			MenuItem openItem = new MenuItem {
				Header = "Open Input",
			};
			MenuItem quitItem = new MenuItem {
				Header = "Quit Application",
			};

			quitItem.Click += QuitItem_Click;
			openItem.Click += OpenItem_Click;

			icon.ContextMenu.Items.Add(openItem);
			icon.ContextMenu.Items.Add(quitItem);
		}

		private void OpenItem_Click(object sender, RoutedEventArgs e) {
			App.mapper.Tray(sender, e);
		}

		private void QuitItem_Click(object sender, RoutedEventArgs e) {
			Environment.Exit(0);
		}
	}
}
