using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuickerAccess {
	class CommandManager {
		internal Dictionary<string, string> directoryOpenners = new Dictionary<string, string>();
		internal Dictionary<string, string> fileOpenners = new Dictionary<string, string>();
		internal Dictionary<string, string> clipboardSwapper = new Dictionary<string, string>();

		internal string clipboardHistory;

		public CommandManager() {
			CommandParser.Parse(this);
		}

		public void HandleCommand(string command) {
			if (directoryOpenners.ContainsKey(command)) {
				new Process {
					StartInfo = new ProcessStartInfo(directoryOpenners[command])
				}.Start();
			}
			else if (fileOpenners.ContainsKey(command)) {
				new Process {
					StartInfo = new ProcessStartInfo(fileOpenners[command])
				}.Start();
			}
			else if (clipboardSwapper.ContainsKey(command)) {
				clipboardHistory = Clipboard.GetText();
				Clipboard.SetText(clipboardSwapper[command]);
			}
		}
	}
}
