using System.Windows.Forms;

namespace QuickerAccess {
	interface ICommand {
		CommandCategory cathegory { get; }
		CommandType type { get; }

		ICommand Parse(string[] splitLine);

		void Execute();
	}
}
