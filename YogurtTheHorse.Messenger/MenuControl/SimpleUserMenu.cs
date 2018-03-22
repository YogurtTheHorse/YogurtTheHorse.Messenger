using System.Linq;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.MenuControl {
    public abstract class SimpleUserMenu<TUserData> : IUserMenu where TUserData : class, IUserData {
        protected MenuController<TUserData> _menuController;

        protected abstract ButtonLayout Layout { get; }
        protected abstract string StartMessage { get; }

        public abstract string MenuName { get; }

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

            ButtonInfo bi = Layout.GetButtons().FirstOrDefault(b => b.Text == message.Text);

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
    }
}
