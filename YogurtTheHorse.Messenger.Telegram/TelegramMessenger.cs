using System;
using System.Threading.Tasks;
using NLog;

using Telegram.Bot;
using Telegram.Bot.Types;
using YogurtTheHorse.Messenger.Database;

namespace YogurtTheHorse.Messenger.Telegram {
    public sealed class TelegramMessenger : IMessenger  {
        private static Logger _logger = LogManager.GetLogger("telegram-messenger");
        internal const string PLATFORM_NAME = "Telegram";

        private TelegramBotClient _telegramBotClient;

        public IDatabaseDriver Database { get; private set; }
        public string PlatformName => PLATFORM_NAME;
        public event MessageEventHandler OnIncomingMessage;
 

        public TelegramMessenger(string token, IDatabaseDriver databaseDriver) {
            Database = databaseDriver;
            _telegramBotClient = new TelegramBotClient(token);
        }

        public async Task<User> GetUserAsync(string id) {
            return await Database.GetUserAsync(id);
        }

        private async Task<User> GetUserAsync(global::Telegram.Bot.Types.User tlgrmUser) {
            User usr = await GetUserAsync(tlgrmUser.Id.ToString());

            if (usr == null) {
                usr = new User(this, tlgrmUser.Id.ToString());
            }

            if (tlgrmUser.UpdateUser(usr)) { await usr.Save(); }

            return usr;
        }

        public async Task<bool> SaveUserAsync(User usr) {
            return await Database.SaveUserAsync(usr);
        }

        public void Launch() {
            _telegramBotClient.OnMessage += (s, e) => OnIncomingMessage?.Invoke(CreateMessage(e.Message));

            var botInfo = _telegramBotClient.GetMeAsync().Result;

            _logger.Info($"Working with ${botInfo.Username}.");
            _telegramBotClient.StartReceiving();
        }

        private Message CreateMessage(global::Telegram.Bot.Types.Message message, ImageInfo image = null) {
            Task<User> usrTask = GetUserAsync(message.From);
            usrTask.Start();
            usrTask.Wait();

            return new Message() {
                MessageType = message.Type.ToYogurtType(),
                Text = message.Text,
                Recipient = usrTask.Result,
                ImageInfo = image
            };
        }

        public void SendMessage(Message message) {
            var msgTask = SendMessageAsync(message);
            msgTask.Start();
            msgTask.Wait();
        }

        public async Task SendMessageAsync(Message message) {
            ChatId chatId = new ChatId(message.Recipient.ID);
            switch (message.MessageType) {
                case MessageType.Text:
                    await _telegramBotClient.SendTextMessageAsync(chatId, message.Text);
                    break;

                case MessageType.Image:
                    await _telegramBotClient.SendPhotoAsync(chatId, Extensions.ImageToFileToSend(message.ImageInfo));
                    break;

                case MessageType.Other:
                    _logger.Error("Unknow message type got.");
                    break;
            }
        }
    }
}
