using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace QuickerAccess {
	class CommandManager {
		internal Dictionary<string, string> directoryOpenners = new Dictionary<string, string>();
		internal Dictionary<string, string> fileOpenners =		new Dictionary<string, string>();
		internal Dictionary<string, string> clipboardSwapper =	new Dictionary<string, string>();

		internal string clipboardHistory;

		public CommandManager() {
			CommandParser.Parse(this);
		}

		/// <summary>
		/// Handle Command according to its type
		/// </summary>
		public void HandleCommand(string command) {
			CommandType type = GetCommandType(command);

			switch (type) {
				case CommandType.FileOpenner: {
					RunDefaultProcess(fileOpenners[command]);
					break;
				}
				case CommandType.FolderOpenner: {
					RunDefaultProcess(directoryOpenners[command]);
					break;
				}
				case CommandType.ClipboardSwapper: {
					clipboardHistory = Clipboard.GetText();
					Clipboard.SetText(clipboardSwapper[command]);
					break;
				}
			}
		}

		/// <summary>
		/// Get command type from a command string
		/// </summary>
		internal CommandType GetCommandType(string command) {
			if (directoryOpenners.ContainsKey(command)) {
				return CommandType.FolderOpenner;
			}
			if(fileOpenners.ContainsKey(command)) {
				return CommandType.FileOpenner;
			}
			if (clipboardSwapper.ContainsKey(command)) {
				return CommandType.ClipboardSwapper;
			}
			return CommandType.None;
		}

		/// <summary>
		/// Run default process, that uses default app to process argument
		/// </summary>
		internal void RunDefaultProcess(string command) {
			new Process {
				StartInfo = new ProcessStartInfo(command)
			}.Start();
		}
	}
}
