using System.Windows.Forms;

namespace QuickerAccess {
	internal class ClipboardModifier : TextCommand {

		internal string clipboardText;

		public override void Execute() {
			Clipboard.SetText(clipboardText);
		}

		public override ICommand Parse(string[] splitLine) {
			// [0] is the base type ID
			// [1] is this classes Type
			// [2] is the text to expect as a trigger
			// [3] is the text to put into Clipboard
			textTrigger = splitLine[2].Trim();
			clipboardText = splitLine[3].Trim();
			return this;
		}
	}
}
