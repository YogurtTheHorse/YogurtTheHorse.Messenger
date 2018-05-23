using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using Commentgram.Bot.Menus;
using Commentgram.Bot.Menus.AccountMenus;

using YogurtTheHorse.Messenger.Database.Mongo;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.Telegram;

using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using Commentgram.Bot.Layouts;

namespace Commentgram.Bot {
	public class Program {
		public static void Main(string[] args) {
			var tokensConfigurationBuilder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("tokens.json");

			IConfiguration tokensConfigutaion = tokensConfigurationBuilder.Build();

			var mongoDriver = new MongoDriver("commentgram");
			mongoDriver.RegisterUserDataClass<CommentgramUserData>();
			var telegramMessenger = new TelegramMessenger(tokensConfigutaion["telegram_token"], mongoDriver);

			var menuController = new MenuController(telegramMessenger, (s) => new CommentgramUserData(s));

			menuController.RegisterMenuClass<AccountMenu>();
			menuController.RegisterMenuClass<MainMenu>();
			menuController.RegisterMenuClass<InstagramCreditanalsMenu>();

			menuController.RegisterMenuClass<MoneyMenu>();

			mongoDriver.Connect();
			telegramMessenger.Launch();
			Console.WriteLine("Now listen to music");
			Console.ReadLine();
		}
	}
}
