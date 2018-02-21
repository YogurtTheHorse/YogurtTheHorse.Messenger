namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
    public class NavigationButtonInfo : UsualButtonInfo {
        public override EButtonType ButtonType => EButtonType.Usual;

        public NavigationButtonInfo(string text, string menuToOpen) : base(text) {
            Data = menuToOpen;
        }

        public override void Action<TUserData>(object sender, ButtonActionEventArgs<TUserData> e) {
            e.MenuController.OpenMenu(e.User, e.UserData, Data as string);
        }
    }
}
