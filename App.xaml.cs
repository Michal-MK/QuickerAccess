using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace QuickerAccess {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : System.Windows.Application {
		internal static HotkeyMapper mapper;
		internal static CommandManager manager;

		public static string resourcesPath { get { return @"pack://application:,,,/QuickerAccess;component/Resources/"; } }

		public App() {
			mapper = new HotkeyMapper(Keys.F6, KeyModifiers.Control | KeyModifiers.Alt);
			manager = new CommandManager();
			Hardcodet.Wpf.TaskbarNotification.TaskbarIcon t = new Hardcodet.Wpf.TaskbarNotification.TaskbarIcon();
			t.IconSource = new BitmapImage(new Uri(resourcesPath + "Test.ico"));
			t.TrayLeftMouseDown += mapper.Tray;
		}
	}
}
