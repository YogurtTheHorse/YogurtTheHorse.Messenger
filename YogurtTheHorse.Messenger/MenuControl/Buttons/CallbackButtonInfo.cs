using System;
using System.Collections.Generic;
using System.Text;

namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
	public sealed class CallbackButtonInfo<TUserData> : UsualButtonInfo where TUserData : class, IUserData {
		private Action<object, ButtonActionEventArgs<TUserData>> _callback;
		public override EButtonType ButtonType => EButtonType.Usual;

		public CallbackButtonInfo(string text, Action<object, ButtonActionEventArgs<TUserData>> callback) : base(text) {
			_callback = callback;
		}

		public override void Action<T>(object sender, ButtonActionEventArgs<T> e) {
			_callback(sender, e as ButtonActionEventArgs<TUserData>);
		}

		public static CallbackButtonInfo<TUserData> BackButton(string text, MenuController<TUserData> controller) {
			return new CallbackButtonInfo<TUserData>(text, (s, e) => controller.Back(e.User, e.UserData));
		}
	}
}
