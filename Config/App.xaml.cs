using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace QuickerAccess {
	/// <summary>
	/// Main class, instantiates all sub controls, registers <see cref="HotKeyMapper"/> for dialog opening and creates <see cref="Tray"/>
	/// </summary>
	public partial class App : System.Windows.Application {

		/// <summary>
		/// Reference to global hot-key mapper
		/// </summary>
		internal static MainWindowOpen main;

		/// <summary>
		/// Reference to parser of entered command, carries out response
		/// </summary>
		internal static CommandManager manager;

		/// <summary>
		/// Reference to tray manager
		/// </summary>
		internal static Tray tray;

		internal static int currentKeyboardLayoutID;

		public App() {
			try {
				using (StreamReader sr = new StreamReader("definition.txt")) { }
			}
			catch (IOException) {
				MessageBox.Show("Unable to locate or access 'definition.txt' in programs directory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(-1);
			}
			catch (ArgumentException) {
				MessageBox.Show("Invalid content in 'definition.txt', fix it or re-download it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(-1);
			}
			catch (IndexOutOfRangeException) {
				MessageBox.Show("Missing dialog opening shortcut. See the file for instructions.");
				Environment.Exit(-1);
			}
			manager = new CommandManager();
			tray = new Tray();
		}
	}
}
