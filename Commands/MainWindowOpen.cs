using System;
using System.Windows;
using System.Windows.Forms;

namespace QuickerAccess {
	class MainWindowOpen : HotkeyCommand {

		private MainWindow _windowReference;
		private int _escapeID;
		private int _enterID;
		private int _activationID;
		bool isOpen = false;

		public override void Execute() {
			if (!isOpen) {
				ShowAndReg();
				Activate();
			}
			else {
				HideAndUnreg();
				App.Current.Dispatcher.Invoke(() => (App.Current.MainWindow as MainWindow).MAIN_Command.Text = "");
			}
			isOpen = !isOpen;
		}

		public override ICommand Parse(string[] splitLine) {
			// [0] Category identifier
			// [1] This
			// [2] Key combinations

			string[] split = splitLine[2].Split();
			foreach (string s in split) {
				if (Enum.TryParse(s, out KeyModifiers mods)) {
					modifiers |= mods;
					continue;
				}
				if (Enum.TryParse(s, out Keys key)) {
					mainKey = key;
				}
			}

			_activationID = HotKeyManager.RegisterHotKey(mainKey, modifiers);
			HotKeyManager.HotKeyPressed += HotKeyManager_HotKeyPressed;
			App.main = this;
			return this;
		}

		private void HideAndUnreg() {
			HotKeyManager.UnregisterHotKey(_escapeID);
			HotKeyManager.UnregisterHotKey(_enterID);
			App.Current.Dispatcher.Invoke(() => { _windowReference.Hide(); _windowReference.MAIN_Command.Text = ""; });
			_activationID = HotKeyManager.RegisterHotKey(mainKey, modifiers);
		}

		private void Activate() {
			_windowReference.Show();
			_windowReference.Activate();
			_windowReference.MAIN_Command.Focus();
		}

		private void ShowAndReg() {
			_escapeID = HotKeyManager.RegisterHotKey(Keys.Escape, KeyModifiers.None);
			_enterID = HotKeyManager.RegisterHotKey(Keys.Enter, KeyModifiers.None);
			HotKeyManager.UnregisterHotKey(_activationID);
			if (_windowReference == null) {
				App.Current.Dispatcher.Invoke(() => {
					System.Windows.Application.Current.MainWindow = _windowReference = new MainWindow();
					Activate();
				});
			}
			else {
				App.Current.Dispatcher.Invoke(() => Activate());
			}
		}

		private void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e) {
			if (e.Key == Keys.Escape) {
				HideAndUnreg();
				return;
			}
			else if (e.Key == Keys.Enter) {
				string command = App.Current.Dispatcher.Invoke(() => ((MainWindow)App.Current.MainWindow).MAIN_Command.Text);

				App.manager.HandleCommand(command);
				HideAndUnreg();
				return;
			}
			if (e.Key == mainKey && e.Modifiers == modifiers) {
				ShowAndReg();
			}
			else {
				App.manager.HandleCommand(e.Key, e.Modifiers);
			}
		}

		internal void Tray(object sender, RoutedEventArgs e) {
			ShowAndReg();
		}
	}
}
