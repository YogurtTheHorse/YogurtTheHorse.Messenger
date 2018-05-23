using System.Threading.Tasks;
using YogurtTheHorse.Messenger.MenuControl.Buttons;
using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;

namespace YogurtTheHorse.Messenger {
	public sealed class User {
		public IMessenger Messenger { get; set; }

		public string PlatformName => Messenger.PlatformName;

		public string UserID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string LanguageCode { get; set; }

		public User(string id) {
			UserID = id;
		}

		public User(IMessenger messenger, string id) : this(id) {
			Messenger = messenger;
		}

		public async Task<bool> Save() {
			return await Messenger.SaveUserAsync(this);
		}

		public void SendMessage(string text, ButtonLayout layout) {
			AsyncHelpers.RunSync(() => SendMessageAsync(text, layout));
		}

		public void SendMessage(string text) {
			SendMessage(text, Builders.LayoutBuilder.Empty);
		}

		public async Task SendMessageAsync(string text, ButtonLayout layout) {
			Message message = new Message() {
				Text = text,
				Recipient = this,
				Layout = layout
			};

			await Messenger.SendMessageAsync(message);
		}
	}
}