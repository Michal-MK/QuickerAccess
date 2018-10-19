using System;
using System.IO;
using System.Windows.Forms;

namespace QuickerAccess {
	/// <summary>
	/// Main class, instantiates all sub controls, registers <see cref="HotKeyMapper"/> for dialog opening and creates <see cref="Tray"/>
	/// </summary>
	public partial class App : System.Windows.Application {

		/// <summary>
		/// Reference to global hotkey mapper
		/// </summary>
		internal static HotKeyMapper mapper;

		/// <summary>
		/// Reference to parser of entered command, carries out response
		/// </summary>
		internal static CommandManager manager;

		/// <summary>
		/// Reference to tray manager
		/// </summary>
		internal static Tray tray;

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
					mapper = new HotKeyMapper(mainKey, opt1 | opt2);
				}
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
