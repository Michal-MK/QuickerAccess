using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace QuickerAccess {

	/// <summary>
	/// Helper function to parse text from 'definition.txt' file
	/// </summary>
	static class CommandParser {

		internal static readonly Dictionary<string,string> knownKeys = new Dictionary<string, string> {
			{ "MAIN", nameof(QuickerAccess) +"."+  nameof(MainWindowOpen) }, //Command to open the window for typing commands
			{ "CZEN", nameof(QuickerAccess) +"."+  nameof(LangFixer) },//Command to replace highlighted text with correct one in the other language
			{ "OFO", nameof(QuickerAccess) +"."+ nameof(OpenFolderCommand) },//Open folder
			{ "OFI", nameof(QuickerAccess) +"."+ nameof(OpenFileCommand)},//Open file
			{ "CLP", null},//Modify clipboard
		};

		/// <summary>
		/// Parses the file into 'manager'
		/// </summary>
		internal static List<ICommand> Parse(CommandManager manager) {
			string[] lines = File.ReadAllLines("definition.txt");
			lines = lines.SelectiveRemove((string s) => s.StartsWith("#") || string.IsNullOrWhiteSpace(s));
			Assembly a = Assembly.GetCallingAssembly();

			List<ICommand> commads = new List<ICommand>();

			for (int i = 0; i < lines.Length; i++) {
				string[] s = lines[i].Split('|');
				switch (s[0]) {
					case "K": {
						commads.Add((Activator.CreateInstance(a.GetType(knownKeys[s[1]])) as TextCommand).Parse(s));
						break;
					}
					case "H": {
						commads.Add((Activator.CreateInstance(a.GetType(knownKeys[s[1]])) as HotkeyCommand).Parse(s));
						break;
					}
				}
			}
			return commads;
		}
	}
}
