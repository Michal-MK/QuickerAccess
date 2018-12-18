using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickerAccess {
	class ExitApplication : TextCommand {
		public override void Execute() {
			App.ExitApp();
		}

		public override ICommand Parse(string[] splitLine) {
			// [0] is the base type ID
			// [1] is this classes Type
			// [2] is the text to expect as a trigger
			textTrigger = splitLine[2].Trim();
			return this;
		}
	}
}
