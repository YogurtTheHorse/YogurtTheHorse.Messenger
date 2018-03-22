using Commentgram.Bot.Menus;
using System;
using YogurtTheHorse.Messenger.Database;
using YogurtTheHorse.Messenger.Database.Mongo;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.Telegram;

namespace Commentgram.Bot {
    public class Program {
        public static void Main(string[] args) {
			IDatabaseDriver mongoDriver = new MongoDriver<CommentgramUserData>("commentgram");
			mongoDriver.Connect();
            TelegramMessenger telegramMessenger = new TelegramMessenger("191656458:AAFHuRzACeNHnNf23ATEkuwJF4fuU7mFjZQ", mongoDriver);
            MenuController<CommentgramUserData> menuController = new MenuController<CommentgramUserData>(telegramMessenger);
			menuController.RegisterMenuInstance(new MainMenu(menuController));
			menuController.RegisterMenuInstance(new AccountMenu(menuController));

			telegramMessenger.Launch();
			Console.WriteLine("Now listen to music");
			Console.ReadLine();
        }
    }
}
