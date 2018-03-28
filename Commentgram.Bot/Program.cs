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

			MenuController menuController = Builders.MenuControllerBuilder.
				Messenger(telegramMessenger).
				GenerateUserData((s) => new CommentgramUserData(s)).

				AddMenu(Builders.MenuBuilder.
					Name("MainMenu").
					Layout(new MainMenuLayout()).
					StartMessage("{MainMenu.StartMessage}").
					Build()).

				AddMenu(Builders.MenuBuilder.
					Name("AccountMenu").
					StartMessage("{AccountMenu.StartMessage}").
					Layout(Builders.LayoutBuilder.
							AddButton(new ButtonInfoBuilder().
								NavigateTo<WalletMenu>().
								Text("{AccountMenu.WalletNumberMenu}")).

							AddButton(new ButtonInfoBuilder().
								NavigateTo<MoneyMenu>().
								Text("{AccountMenu.MoneyMenu}")).

						NextRow().
							AddButton(ButtonInfoBuilder.BackButton)).
					Build()).

				AddMenu<MoneyMenu>().
				AddMenu<WalletMenu>().

				Build();

			mongoDriver.Connect();
			telegramMessenger.Launch();
			Console.WriteLine("Now listen to music");
			Console.ReadLine();
		}
	}
}
