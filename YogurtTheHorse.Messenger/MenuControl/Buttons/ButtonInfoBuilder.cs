using System;
using System.Collections.Generic;
using System.Text;

namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
	public class ButtonInfoBuilder {
		protected EButtonType _buttonType = EButtonType.Usual;
		protected string _text;
		protected string _data;
		protected Action<object, ButtonActionEventArgs> _callback;
		protected Func<User, UserData, bool> _condition = (u, ud) => false;

		public ButtonInfoBuilder Text(string text) {
			_text = text ?? throw new ArgumentNullException(nameof(text));
			return this;
		}

		public ButtonInfoBuilder ButtonType(EButtonType buttonType) {
			_buttonType = buttonType;

			return this;
		}

		public ButtonInfoBuilder Data(string data) {
			_data = data;

			return this;
		}

		public ButtonInfoBuilder HideCondition(Func<User, UserData, bool> condition) {
			_condition = condition;

			return this;
		}


		public ButtonInfoBuilder Callback(Action<object, ButtonActionEventArgs> callback) {
			_callback = callback;

			return this;
		}

		public ButtonInfoBuilder BackCallback() {
			_callback = (s, e) => e.MenuController.Back(e.User, e.UserData);

			return this;
		}

		public ButtonInfoBuilder NavigateTo(string menuName) {
			_callback = (s, e) => e.MenuController.OpenMenu(e.User, e.UserData, menuName);

			return this;
		}

		public ButtonInfoBuilder NavigateTo<TUserMenu>() where TUserMenu : IUserMenu {
			_callback = (s, e) => e.MenuController.OpenMenu(e.User, e.UserData, typeof(TUserMenu).Name);

			return this;
		}

		public ButtonInfoBuilder BackButton() {
			return BackCallback().Text("{Back}");
		}

		public virtual ButtonInfo ToButton() {
			ButtonInfo buttonInfo = _callback is null ? new ButtonInfo() : new CallbackButtonInfo(_callback);

			buttonInfo.Data = _data;
			buttonInfo.Text = _text ?? throw new ArgumentNullException(nameof(_text));
			buttonInfo.ButtonType = _buttonType;
			buttonInfo.HideCondition = _condition;

			return buttonInfo;
		}


		public static implicit operator ButtonInfo(ButtonInfoBuilder builder) {
			return builder.ToButton();
		}
	}
}
