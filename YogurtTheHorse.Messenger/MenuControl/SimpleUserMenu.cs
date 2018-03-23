using System.Diagnostics;
using System.Linq;
using System.Reflection;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.MenuControl {
	public abstract class SimpleUserMenu : IUserMenu {
		protected MenuController _menuController;
		protected virtual ButtonLayout Layout { get; set; }

		protected abstract string StartMessage { get; }

		public virtual string MenuName => GetType().Name;

		public SimpleUserMenu(MenuController menuController) {
			_menuController = menuController;
		}

		public virtual void Open(User user, UserData userData, object sender) {
			user.SendMessage(StartMessage, Layout);
		}

		public virtual void OnUnusualMessage(Message message, UserData userData) {
			Open(message.Recipient, userData, _menuController);
		}

		public virtual void OnMessage(Message message, UserData userData) {
			if (Layout.LayoutType != EButtonType.Usual) { return; }

			ButtonInfo bi = Layout.GetAllButtons().FirstOrDefault(b => b.Text == message.Text);

			if (bi is null) {
				OnUnusualMessage(message, userData);
			} else {
				bi.Action(this, new ButtonActionEventArgs() {
					ButtonClickType = EButtonType.Usual,
					Data = bi.Data,
					MenuController = _menuController,
					User = message.Recipient,
					UserData = userData
				});
			}
		}

		public virtual void Close(User user, UserData userData, object sender) { }

		public void Back(User user, UserData userData) => _menuController.Back(user, userData);

		public ButtonInfoBuilder GetNavigationButton() {
			return new ButtonInfoBuilder().NavigateTo(MenuName);
		}
	}
}
