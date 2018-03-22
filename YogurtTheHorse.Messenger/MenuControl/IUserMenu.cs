namespace YogurtTheHorse.Messenger.MenuControl {
    public interface IUserMenu {
        string MenuName { get; }
        void OnMessage(Message message, IUserData userData);
        void Open(User user, IUserData userData, object sender);
        void Close(User user, IUserData userData, object sender);

		void OnUnusualMessage(Message message, IUserData userData);
	}
}
