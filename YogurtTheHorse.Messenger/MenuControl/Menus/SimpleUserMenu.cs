using System.Linq;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.MenuControl.Menus {
	public abstract class SimpleUserMenu : IUserMenu {
		protected virtual ButtonLayout Layout { get; set; }

		protected virtual string StartMessage { get; }

		public virtual string MenuName => GetType().Name;

		public virtual void Open(User user, UserData userData, object sender) {
			user.SendMessage(StartMessage, Layout.GetButtons(user, userData));
		}

		public virtual void OnUnusualMessage(Message message, UserData userData) {
			Open(message.Recipient, userData, message.Controller);
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
					MenuController = message.Controller,
					User = message.Recipient,
					UserData = userData
				});
			}
		}

		public virtual void Close(User user, UserData userData, object sender) { }
	}
}
