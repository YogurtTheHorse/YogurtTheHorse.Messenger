using System;
using YogurtTheHorse.Messenger.MenuControl;

namespace Commentgram.Bot {
	public class CommentgramUserData : IUserData {
		public string UserID { get; set; }
		public string MenuName { get; set; }
        public string Locale { get; set; }

		public string InstagramLogin { get; set; }
		public string InstagramPassowrd { get; set; }

		public CommentgramUserData(string userID) {
			UserID = userID;
			MenuName = "MainMenu";
			Locale = "ru";
		}
	}
}