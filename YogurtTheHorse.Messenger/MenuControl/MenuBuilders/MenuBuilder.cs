using System;
using YogurtTheHorse.Messenger.MenuControl.Buttons;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace YogurtTheHorse.Messenger.MenuControl.MenuBuilders {
	public sealed class MenuBuilder {
		private ButtonLayout _layout;
		private Action<User, UserData, object> _closeAction, _openAction;
		private Action<Message, UserData> _onUnusualMessageAction;
		private string _menuName;

		public MenuBuilder() {
			_layout = new LayoutBuilder().Build();
		}

		public MenuBuilder Name(string name) {
			_menuName = name ?? throw new ArgumentNullException(nameof(name));
			return this;
		}

		public MenuBuilder Layout(ButtonLayout layout) {
			_layout = layout ?? throw new ArgumentNullException(nameof(layout));
			return this;
		}

		public MenuBuilder OnClose(Action<User, UserData, object> close) {
			_closeAction = close;
			return this;
		}

		public MenuBuilder OnOpen(Action<User, UserData, object> open) {
			_openAction = open;
			return this;
		}

		public MenuBuilder OnUnusualMessage(Action<Message, UserData> onUnusualMessage) {
			_onUnusualMessageAction = onUnusualMessage;
			return this;
		}

		public SimpleUserMenu Build() {
			return new BuildedMenu(
				_menuName,
				_layout,
				_closeAction,
				_onUnusualMessageAction,
				_openAction);
		}

		public static implicit operator SimpleUserMenu(MenuBuilder builder) {
			return builder.Build();
		}
	}
}
