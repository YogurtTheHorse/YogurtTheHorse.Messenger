using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;
using YogurtTheHorse.Messenger.Database;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.ConsoleMessenger {
	class ConsoleMessenger : IMessenger {
		private static Logger _logger = LogManager.GetLogger("telegram-messenger");
		internal const string PLATFORM_NAME = "Console";

		public IDatabaseDriver Database { get; }
		public string PlatformName => PLATFORM_NAME;
		public event MessageEventHandler OnIncomingMessage;

		private string _username;

		public ConsoleMessenger(string username, IDatabaseDriver databaseDriver) {
			Database = databaseDriver;
			_username = username;
		}

		public async Task<User> GetUserAsync(string id) {
			User user = await Database.GetUserAsync(id);
			if (!(user is null)) {
				user.Messenger = this;
			}

			return user;
		}

		public async Task<bool> SaveUserAsync(User usr) {
			return await Database.SaveUserAsync(usr);
		}

		public async Task Launch() {
			_logger.Info($"Working");

			while (true) {
				string s = Console.ReadLine();
				OnIncomingMessage?.Invoke(await CreateMessageAsync(s));
			}

		}

		private async Task<Message> CreateMessageAsync(string s) {
			return new Message() {
				MessageType = MessageType.Text,
				Text = s,
				Recipient = await GetUserAsync(_username),
				ImageInfo = null
			};
		}

		public void SendMessage(Message message) {
			switch (message.MessageType) {
				case MessageType.Text:
					Console.WriteLine(message.Text);
					ButtonInfo[][] buttons = message.Layout.GetButtons();

					for (int i = 0; i < buttons.Length; i++) {
						var textButtons = 
							from b in buttons[i]
							where !b.HideCondition(message.Recipient, Database.GetUserData(_username))
							select $"[ {b.Text} {b.ButtonType == EButtonType.Inline ? $", {b.Data}" : String.Empty} ]";

						Console.Write($"[ {String.Join(", ", textButtons)} ]");
					}
					break;

				case MessageType.Image:
					_logger.Info("Images not supported");
					break;

				case MessageType.Other:
					_logger.Error("Unknow message type got.");
					break;
			}
		}

		public async Task SendMessageAsync(Message message) {
			await new Task(() => {
				SendMessage(message);
			});
		}

		void IMessenger.Launch() {
			Launch().RunSynchronously();
		}
	}
}