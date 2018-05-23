using MongoDB.Bson.Serialization.Attributes;
using System;
using YogurtTheHorse.Messenger.MenuControl;

namespace Commentgram.Bot {
	public class CommentgramUserData : UserData {
		public string InstagramLogin { get; set; }
		public string InstagramPassword { get; set; }
		public bool IsInstagramAuthinticated { get; set; }
		
		public FollowingType FollowingType { get; set; } 

		public string YandexWallet { get; set; }
		public decimal Amount { get; set; }

		public string LinkToAdd { get; set; }

		public CommentgramUserData(string userID) : base(userID) {
			MenuStack.Push("MainMenu");
			Locale = "ru";
			Amount = 0;
		}

		public void CheckInstagramAuth() {
			Console.WriteLine(InstagramLogin + " " + InstagramPassword);
		}
	}
}