using System;
using System.Collections.Generic;
using YogurtTheHorse.Messenger.MenuControl;

namespace Commentgram.Bot {
	public class CommentgramUserData : UserData {
		public string InstagramLogin { get; set; }
		public string InstagramPassowrd { get; set; }

		public string YandexWallet { get; set; }

		public decimal Amount { get; set; }

		public CommentgramUserData(string userID) : base(userID) {
			MenuStack.Push("MainMenu");
			Locale = "ru";
			Amount = 0;
		}
	}
}