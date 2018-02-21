using Commentgram.Bot.Menus;

using YogurtTheHorse.Messenger.Database.Mongo;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.Telegram;

namespace Commentgram.Bot {
    public class Program {
        public static void Main(string[] args) {
            MongoDriver mongoDriver = new MongoDriver("commentgram");
            TelegramMessenger telegramMessenger = new TelegramMessenger("<TOKEN>", mongoDriver);
            MenuController<CommentgramUserData> menuController = new MenuController<CommentgramUserData>(telegramMessenger);
            menuController.RegisterMenuInstance(new MainMenu(menuController));

            telegramMessenger.Launch();
        }
    }
}
