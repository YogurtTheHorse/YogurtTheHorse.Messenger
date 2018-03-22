namespace YogurtTheHorse.Messenger.MenuControl {
    public interface IUserData {
        string UserID { get; }
        string MenuName { get; set; }
		string Locale { get; set; }
    }
}
