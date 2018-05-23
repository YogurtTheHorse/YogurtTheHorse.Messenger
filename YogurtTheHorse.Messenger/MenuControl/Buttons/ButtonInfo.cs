using System;
using YogurtTheHorse.Messenger.Database;

namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
	public class ButtonInfo {
		public virtual EButtonType ButtonType { get; set; }
		public string Text { get; protected internal set; }
		public string Data { get; protected internal set; }

		public Func<User, UserData, bool> HideCondition { get; protected internal set; }

		public ButtonInfo() {
			ButtonType = EButtonType.Usual;
			HideCondition = (u, ud) => false;
		}

		public virtual void Action(object sender, ButtonActionEventArgs e) { }
	}

	public class ButtonActionEventArgs : EventArgs  {
		public User User { get; set; }
		public UserData UserData { get; set; }
		public MenuController MenuController { get; set; }

		public IDatabaseDriver Database => Messenger.Database;
		public IMessenger Messenger => MenuController.Messenger;

		public EButtonType ButtonClickType { get; set; }
		public object Data { get; set; }
	}
}
