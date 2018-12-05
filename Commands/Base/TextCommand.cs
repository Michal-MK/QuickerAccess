using System.Windows.Forms;

namespace QuickerAccess {
	internal abstract class TextCommand : ICommand {
		public CommandCategory cathegory => CommandCategory.InputTextShortcut;

		public CommandType type { get; set; }

		public string textTrigger { get; set; }

		public abstract void Execute();
		public abstract ICommand Parse(string[] splitLine);
	}
}
