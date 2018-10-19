using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace QuickerAccess {
	/// <summary>
	/// Parses entered string from the dialog, responds to the command
	/// </summary>
	class CommandManager {

		/// <summary>
		/// Holds all folder opening commands
		/// </summary>
		internal Dictionary<string, string> directoryOpenners = new Dictionary<string, string>();

		/// <summary>
		/// Holds all file opening commands
		/// </summary>
		internal Dictionary<string, string> fileOpenners =		new Dictionary<string, string>();

		/// <summary>
		/// Holds all string to replace clipboard with.
		/// </summary>
		internal Dictionary<string, string> clipboardSwapper =	new Dictionary<string, string>();

		internal string _previousClipboardContent;

		/// <summary>
		/// Default constructor, parses 'definition.txt' file
		/// </summary>
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
					_previousClipboardContent = Clipboard.GetText();
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
		/// Run default process, that uses default application to process argument
		/// </summary>
		internal void RunDefaultProcess(string command) {
			new Process {
				StartInfo = new ProcessStartInfo(command)
			}.Start();
		}
	}
}
