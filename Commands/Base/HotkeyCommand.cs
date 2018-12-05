using System.Windows.Forms;

namespace QuickerAccess {
	internal abstract class HotkeyCommand : ICommand {
		public KeyModifiers modifiers { get; set; }

		public Keys mainKey { get; set; }

		public CommandCategory cathegory => CommandCategory.KeyboardHotkey;

		public CommandType type { get; set; }

		public abstract void Execute();
		public abstract ICommand Parse(string[] splitLine);
	}
}
