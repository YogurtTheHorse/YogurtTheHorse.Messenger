using System.Collections.Generic;

namespace YogurtTheHorse.Messenger.MenuControl {
    public interface IUserData {
        string UserID { get; set; }
		string Locale { get; set; }

		Stack<string> MenuStack { get; set; }
    }
}
