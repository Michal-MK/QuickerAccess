using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace QuickerAccess {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : System.Windows.Application {
		internal static HotkeyMapper mapper;
		internal static CommandManager manager;

		public App() {
			try {
				using (StreamReader sr = new StreamReader("definition.txt")) {
					string line = sr.ReadLine();
					while (!line.StartsWith("EXP:")) {
						line = sr.ReadLine();
					}

					string[] split = line.Substring(line.IndexOf(':') + 1).Split();

					if (!Enum.TryParse(split[0], out Keys mainKey)) {
						throw new ArgumentException();
					}
					KeyModifiers opt1 = KeyModifiers.None;
					KeyModifiers opt2 = KeyModifiers.None;

					if (split.Length == 2) {
						if (!Enum.TryParse(split[1], out opt1))
							throw new ArgumentException();
					}
					else if (split.Length == 3) {
						if (!Enum.TryParse(split[2], out opt2))
							throw new ArgumentException();
					}
					mapper = new HotkeyMapper(mainKey, opt1 | opt2);
				}
			}
			catch (IOException) {
				MessageBox.Show("Unable to locate or access 'definition.txt' in programs directory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(-1);
			}
			catch (ArgumentException) {
				MessageBox.Show("Invalid content in 'definition.txt', fix it or redownload it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(-1);
			}
			catch (IndexOutOfRangeException) {
				MessageBox.Show("Missing dialog opening shortcut. See the file for instructions.");
				Environment.Exit(-1);
			}
			manager = new CommandManager();
			Hardcodet.Wpf.TaskbarNotification.TaskbarIcon t = new Hardcodet.Wpf.TaskbarNotification.TaskbarIcon {
				IconSource = new BitmapImage(new Uri(AppHelper.resourcesPath + "TrayIcon.ico"))
			};
			t.TrayLeftMouseDown += mapper.Tray;
		}
	}
}
