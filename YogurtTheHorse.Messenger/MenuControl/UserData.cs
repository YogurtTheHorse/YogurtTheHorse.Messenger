using System.Collections.Generic;

namespace YogurtTheHorse.Messenger.MenuControl {
    public class UserData {
        public string UserID { get; set; }
		public string Locale { get; set; }

		public Stack<string> MenuStack { get; set; }

		public UserData() { }

		public UserData(string id) {
			UserID = id;
			MenuStack = new Stack<string>();
		}
    }
}
