using System;

namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
	public class ButtonInfo {
		public virtual EButtonType ButtonType { get; set; }
		public string Text { get; set; }
		public string Data { get; set; }

		public virtual bool Hide => false;

		public virtual void Action(object sender, ButtonActionEventArgs e) { }
	}

	public class ButtonActionEventArgs : EventArgs  {
		public User User { get; set; }
		public UserData UserData { get; set; }
		public MenuController MenuController { get; set; }

		public EButtonType ButtonClickType { get; set; }
		public object Data { get; set; }
	}
}
