using System.Diagnostics;
using System.Linq;
using System.Reflection;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.MenuControl {
	public abstract class SimpleUserMenu<TUserData> : IUserMenu where TUserData : class, IUserData {
		protected MenuController<TUserData> _menuController;
		protected virtual ButtonLayout Layout { get; set; }

		protected abstract string StartMessage { get; }

		public virtual string MenuName => GetType().Name;

		public SimpleUserMenu(MenuController<TUserData> menuController) {
			_menuController = menuController;
		}

		public virtual void Open(User user, IUserData userData, object sender) {
			user.SendMessage(StartMessage, Layout);
		}

		public void OnMessage(Message message, IUserData userData) {
			OnMessage(message, (TUserData)userData);
		}

		public void OnUnusualMessage(Message message, IUserData userData) {
			OnUnusualMessage(message, (TUserData)userData);
		}
		public virtual void OnUnusualMessage(Message message, TUserData userData) {
			Open(message.Recipient, userData, _menuController);
		}

		public virtual void OnMessage(Message message, TUserData userData) {
			if (Layout.LayoutType != EButtonType.Usual) { return; }

			ButtonInfo bi = Layout.GetAllButtons().FirstOrDefault(b => b.Text == message.Text);

			if (bi is null) {
				OnUnusualMessage(message, userData);
			} else {
				bi.Action(this, new ButtonActionEventArgs<TUserData>() {
					ButtonClickType = EButtonType.Usual,
					Data = bi.Data,
					MenuController = _menuController,
					User = message.Recipient,
					UserData = userData
				});
			}
		}

		public virtual void Close(User user, IUserData userData, object sender) { }

		public static NavigationButtonInfo Navigate(string text) {
			string className = new StackTrace(false).GetFrame(1).GetMethod().DeclaringType.Name.ToString();
			return new NavigationButtonInfo(text, className);
		}
	}
}
