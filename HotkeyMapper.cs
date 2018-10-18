using System.Windows;
using System.Windows.Forms;

namespace QuickerAccess {
	class HotkeyMapper {

		private MainWindow _windowReference;
		private int _escapeID;
		private int _enterID;
		private int _activationID;

		private readonly Keys _mainKey;
		private readonly KeyModifiers _modifierKey;

		public HotkeyMapper(Keys mainKey, KeyModifiers modifierKey) {
			_mainKey = mainKey;
			_modifierKey = modifierKey;
			_activationID = HotKeyManager.RegisterHotKey(mainKey, modifierKey);
			HotKeyManager.HotKeyPressed += HotKeyManager_HotKeyPressed;
		}

		private void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e) {
			if (e.Key == Keys.Escape) {
				//Do nothing and close the window
				HideAndUnreg();
				return;
			}
			else if (e.Key == Keys.Enter) {
				//Handle Input
				string command = App.Current.Dispatcher.Invoke(() => {
					return ((MainWindow)App.Current.MainWindow).MAIN_Command.Text;
				});

				App.manager.HandleCommand(command);
				HideAndUnreg();
				return;
			}
			ShowAndReg();
		}

		private void HideAndUnreg() {
			HotKeyManager.UnregisterHotKey(_escapeID);
			HotKeyManager.UnregisterHotKey(_enterID);
			App.Current.Dispatcher.Invoke(() => _windowReference.Hide());
			_activationID = HotKeyManager.RegisterHotKey(_mainKey, _modifierKey);
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
				App.Current.Dispatcher.Invoke(() => {
					Activate();
				});
			}
		}

		internal void Tray(object sender, RoutedEventArgs e) {
			ShowAndReg();
		}
	}

}
