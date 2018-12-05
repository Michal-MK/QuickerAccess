using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsInput;

namespace QuickerAccess {
	class LangFixer : HotkeyCommand {

		internal readonly Dictionary<string, string> usTOces = new Dictionary<string, string> {
			{ "2","ě" },
			{ "3","š" },
			{ "4","č" },
			{ "5","ř" },
			{ "6","ž" },
			{ "7","ý" },
			{ "8","á" },
			{ "9","í" },
			{ "0","é" },
			{ "[","ú" },
			{ ";","ů" }
		};


		public async override void Execute() {
			InputSimulator iSim = new InputSimulator();
			KeyboardSimulator ks = iSim.Keyboard as KeyboardSimulator;

			ks.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_C);
			await Task.Delay(20);
			string t = Clipboard.GetText();
			foreach (var item in usTOces) {
				t = t.Replace(item.Key, item.Value);
			}
			Clipboard.SetText(t);
			ks.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_V);
			await Task.Delay(20);
			Clipboard.Clear();
		}

		public override ICommand Parse(string[] splitLine) {
			string[] split = splitLine[2].Trim().Split();
			foreach (string s in split) {
				if (Enum.TryParse(s, out KeyModifiers mods)) {
					modifiers |= mods;
					continue;
				}
				if (Enum.TryParse(s, out Keys key)) {
					mainKey = key;
				}
			}
			return this;
		}
	}
}
