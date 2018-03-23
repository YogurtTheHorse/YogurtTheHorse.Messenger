using System;
using System.Collections.Generic;
using System.Text;

namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
	public sealed class CallbackButtonInfo : ButtonInfo {
		private Action<object, ButtonActionEventArgs> _callback;

		public CallbackButtonInfo(Action<object, ButtonActionEventArgs> callback) {
			_callback = callback;
		}

		public override void Action(object sender, ButtonActionEventArgs e) {
			_callback(sender, e);
		}
	}
}
