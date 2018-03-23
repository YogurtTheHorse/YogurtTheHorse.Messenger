using System;

using Commentgram.Bot.Menus;
using Commentgram.Bot.Menus.AccountMenus;
using YogurtTheHorse.Messenger.Database.Mongo;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.Telegram;

namespace Commentgram.Bot {
    public class Program {
        public static void Main(string[] args) {
			var mongoDriver = new MongoDriver("commentgram");
			mongoDriver.RegisterUserDataClass<CommentgramUserData>();
            var telegramMessenger = new TelegramMessenger("191656458:AAFHuRzACeNHnNf23ATEkuwJF4fuU7mFjZQ", mongoDriver);

            MenuController menuController = new MenuController(telegramMessenger, (s) => new CommentgramUserData(s));
			menuController.RegisterMenuInstance(new MainMenu(menuController));
			menuController.RegisterMenuInstance(new AccountMenu(menuController));
			menuController.RegisterMenuInstance(new WalletMenu(menuController));

			mongoDriver.Connect();
			telegramMessenger.Launch();
			Console.WriteLine("Now listen to music");
			Console.ReadLine();
        }
    }
}
