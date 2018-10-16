using System.Windows;
using System.Windows.Forms;

namespace QuickerAccess {
	class HotkeyMapper {

		private MainWindow windowReference;
		private int escapeID;
		private int enterID;
		private int activationID;

		private readonly Keys mainKey;
		private readonly KeyModifiers modifierKey;

		public HotkeyMapper(Keys mainKey, KeyModifiers modifierKey) {
			this.mainKey = mainKey;
			this.modifierKey = modifierKey;
			activationID = HotKeyManager.RegisterHotKey(mainKey, modifierKey);
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
			HotKeyManager.UnregisterHotKey(escapeID);
			HotKeyManager.UnregisterHotKey(enterID);
			App.Current.Dispatcher.Invoke(() => windowReference.Hide());
			activationID = HotKeyManager.RegisterHotKey(mainKey, modifierKey);
		}

		private void Activate() {
			windowReference.Show();
			windowReference.Activate();
			windowReference.MAIN_Command.Focus();
		}

		private void ShowAndReg() {
			escapeID = HotKeyManager.RegisterHotKey(Keys.Escape, KeyModifiers.None);
			enterID = HotKeyManager.RegisterHotKey(Keys.Enter, KeyModifiers.None);
			HotKeyManager.UnregisterHotKey(activationID);
			if (windowReference == null) {
				App.Current.Dispatcher.Invoke(() => {
					System.Windows.Application.Current.MainWindow = windowReference = new MainWindow();
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
