using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickerAccess {

	/// <summary>
	/// Helper function to parse text from 'definition.txt' file
	/// </summary>
	static class CommandParser {

		/// <summary>
		/// Parses the file into 'manager'
		/// </summary>
		internal static void Parse(CommandManager manager) {
			string[] lines = File.ReadAllLines("definition.txt");

			for (int i = 0; i < lines.Length; i++) {
				if (string.IsNullOrEmpty(lines[i])) {
					continue;
				}

				string s = lines[i];
				CommandType type = GetCommandType(ref s);

				if(type == CommandType.None) {
					continue;
				}

				switch (type) {
					case CommandType.FolderOpenner: {
						int pipeIndex = s.IndexOf('|');
						manager.directoryOpenners.Add(s.Substring(0, pipeIndex).Trim(), s.Substring(pipeIndex + 1).Trim());
						break;
					}
					case CommandType.FileOpenner: {
						int pipeIndex = s.IndexOf('|');
						manager.fileOpenners.Add(s.Substring(0, pipeIndex).Trim(), s.Substring(pipeIndex + 1).Trim());
						break;
					}
					case CommandType.ClipboardSwapper: {
						int pipeIndex = s.IndexOf('|');
						manager.clipboardSwapper.Add(s.Substring(0, pipeIndex).Trim(), s.Substring(pipeIndex + 1).Trim());
						break;
					}
				}
			}
		}

		/// <summary>
		/// Looks at the beginning of a string and decides what type of the command it is
		/// </summary>
		private static CommandType GetCommandType(ref string s) {
			if (s.Contains("OFO:")) {
				s = s.Replace("OFO:", "");
				return CommandType.FolderOpenner;
			}
			else if (s.Contains("OFI:")) {
				s = s.Replace("OFI:", "");
				return CommandType.FileOpenner;
			}
			else if (s.Contains("CLP:")) {
				s = s.Replace("CLP:", "");
				return CommandType.ClipboardSwapper;
			}
			return 0;
		}
	}
}
