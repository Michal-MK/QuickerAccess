namespace QuickerAccess {
	class OpenFileCommand : TextCommand {

		public string filePath;

		public override void Execute() {
			CommandManager.RunDefaultProcess(filePath);
		}

		public override ICommand Parse(string[] splitLine) {
			// [0] is the base type ID
			// [1] is this classes Type
			// [2] is the text to expect as a trigger
			// [3] is the folder path

			textTrigger = splitLine[2].Trim();
			filePath = splitLine[3].Trim();
			return this;
		}
	}
}
