using System;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.MenuControl.Menus {
	public class BuildedMenu : SimpleUserMenu {
		private Action<User, UserData, object> _closeAction, _openAction;
		private Action<Message, UserData> _onUnusualMessageAction;

		public override string MenuName { get; }
		protected override string StartMessage { get; }

		internal BuildedMenu(
				string menuName,
				string startMessage,
				ButtonLayout layout,
				Action<User, UserData, object> closeAction = null,
				Action<Message, UserData> onUnusualMessageAction = null,
				Action<User, UserData, object> openAction = null) {
			Layout = layout ?? throw new ArgumentNullException(nameof(layout));
			MenuName = menuName ?? throw new ArgumentNullException(nameof(menuName));
			StartMessage = startMessage;

			_onUnusualMessageAction = onUnusualMessageAction;
			_closeAction = closeAction;
			_openAction = openAction;
		}

		public override void OnUnusualMessage(Message message, UserData userData) {
			(_onUnusualMessageAction ?? base.OnUnusualMessage).Invoke(message, userData);
		}

		public override void Open(User user, UserData userData, object sender) {
			(_openAction ?? base.Open).Invoke(user, userData, sender);
		}

		public override void Close(User user, UserData userData, object sender) {
			(_closeAction ?? base.Close).Invoke(user, userData, sender);
		}
	}
}
