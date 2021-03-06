﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace QuickerAccess {
	[Flags]
	public enum KeyModifiers {
		None = 0,
		Alt = 1,
		Control = 2,
		Shift = 4,
		Windows = 8,
		NoRepeat = 0x4000
	}

	public static class HotKeyManager {
		private delegate void RegisterHotKeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);
		private delegate void UnRegisterHotKeyDelegate(IntPtr hwnd, int id);

		public static event EventHandler<HotKeyEventArgs> HotKeyPressed;

		private static volatile MessageWindow _wnd;
		private static volatile IntPtr _hwnd;
		private static ManualResetEvent _windowReadyEvent = new ManualResetEvent(false);

		private static int _id = 0;

		static HotKeyManager() {
			Thread messageLoop = new Thread(delegate () {
				Application.Run(new MessageWindow());
			});
			messageLoop.Name = "MessageLoopThread";
			messageLoop.IsBackground = true;
			messageLoop.SetApartmentState(ApartmentState.STA);
			messageLoop.Start();
		}


		public static int RegisterHotKey(Keys key, KeyModifiers modifiers) {
			_windowReadyEvent.WaitOne();
			int id = Interlocked.Increment(ref _id);
			_wnd.Invoke(new RegisterHotKeyDelegate(RegisterHotKeyInternal), _hwnd, id, (uint)modifiers, (uint)key);
			return id;
		}

		public static void UnregisterHotKey(int id) {
			_wnd.Invoke(new UnRegisterHotKeyDelegate(UnRegisterHotKeyInternal), _hwnd, id);
		}


		private static void RegisterHotKeyInternal(IntPtr windowHandle, int id, uint modifiers, uint key) {
			RegisterHotKey(windowHandle, id, modifiers, key);
		}

		private static void UnRegisterHotKeyInternal(IntPtr hwnd, int id) {
			UnregisterHotKey(_hwnd, id);
		}

		private static void OnHotKeyPressed(HotKeyEventArgs e) {
			HotKeyPressed?.Invoke(null, e);
		}


		private class MessageWindow : Form {
			private const int WM_HOTKEY = 0x312;

			public MessageWindow() {
				_wnd = this;
				_hwnd = this.Handle;
				_windowReadyEvent.Set();
			}

			protected override void WndProc(ref Message m) {
				if (m.Msg == WM_HOTKEY) {
					HotKeyEventArgs e = new HotKeyEventArgs(m.LParam);
					OnHotKeyPressed(e);
				}
				base.WndProc(ref m);
			}

			protected override void SetVisibleCore(bool value) {
				// Ensure the window never becomes visible
				base.SetVisibleCore(false);
			}
		}

		[DllImport("user32", SetLastError = true)]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32", SetLastError = true)]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
	}

	public class HotKeyEventArgs : EventArgs {
		public readonly Keys Key;
		public readonly KeyModifiers Modifiers;

		public HotKeyEventArgs(Keys key, KeyModifiers modifiers) {
			Key = key;
			Modifiers = modifiers;
		}

		public HotKeyEventArgs(IntPtr hotKeyParam) {
			uint param = (uint)hotKeyParam.ToInt64();
			Key = (Keys)((param & 0xffff0000) >> 16);
			Modifiers = (KeyModifiers)(param & 0x0000ffff);
		}
	}
}
