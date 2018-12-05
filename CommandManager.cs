using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace QuickerAccess {
	/// <summary>
	/// Parses entered string from the dialog, responds to the command
	/// </summary>
	class CommandManager {

		List<ICommand> commands = new List<ICommand>();
		/// <summary>
		/// Default constructor, parses 'definition.txt' file
		/// </summary>
		public CommandManager() {
			commands = CommandParser.Parse(this);
			foreach (ICommand command in commands) {
				if (command is HotkeyCommand)
					HotKeyManager.RegisterHotKey((command as HotkeyCommand).mainKey, (command as HotkeyCommand).modifiers);
			}
		}

		/// <summary>
		/// Handle Command according to its type
		/// </summary>
		public void HandleCommand(string commandStr) {
			foreach (ICommand command in commands) {
				if (command is TextCommand) {
					if ((command as TextCommand).textTrigger == commandStr) {
						command.Execute();
						return;
					}
				}
			}
		}
		
		/// <summary>
		/// Handle Command according to its type
		/// </summary>
		public void HandleCommand(Keys key, KeyModifiers modifiers) {
			foreach (ICommand command in commands) {
				if (command is HotkeyCommand) {
					if ((command as HotkeyCommand).mainKey == key && (command as HotkeyCommand).modifiers == modifiers) {
						command.Execute();
						return;
					}
				}
			}
		}


		/// <summary>
		/// Run default process, that uses default application to process argument
		/// </summary>
		internal static void RunDefaultProcess(string command) {
			new Process {
				StartInfo = new ProcessStartInfo(command)
			}.Start();
		}
	}
}
