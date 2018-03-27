namespace YogurtTheHorse.Messenger.MenuControl.Menus {
    public interface IUserMenu {
        string MenuName { get; }

        void OnMessage(Message message, UserData userData);
        void Open(User user, UserData userData, object sender);
        void Close(User user, UserData userData, object sender);
	}
}
