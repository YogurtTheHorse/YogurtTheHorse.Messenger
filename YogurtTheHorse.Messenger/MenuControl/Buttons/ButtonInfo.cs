using System;

namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
    public abstract class ButtonInfo {
        public abstract EButtonType ButtonType { get; }
        public string Text { get; set; }
        public string Data { get; set; }

        public abstract void Action<TUserData>(object sender, ButtonActionEventArgs<TUserData> e) where TUserData : class, IUserData;

        public static implicit operator ButtonInfo((string, string) tup) {
            return new NavigationButtonInfo(tup.Item1, tup.Item2);
        }

        public static implicit operator ButtonInfo(string s) {
            return new UsualButtonInfo(s);
        }
    }

    public class ButtonActionEventArgs<TUserData> : EventArgs where TUserData : class, IUserData {
        public User User { get; set; }
        public IUserData UserData { get; set; }
        public MenuController<TUserData> MenuController { get; set; }

        public EButtonType ButtonClickType { get; set; }
        public object Data { get; set; }
    }
}
