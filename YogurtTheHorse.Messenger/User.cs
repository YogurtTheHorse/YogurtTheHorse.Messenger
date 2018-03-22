using System.Threading.Tasks;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger {
    public class User {
        protected IMessenger _messenger;

        public string PlatformName => _messenger.PlatformName;

        public string UserID { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string LanguageCode { get; set; }

        public User(IMessenger messenger, string id) {
            _messenger = messenger;
            UserID = id;
        }

        public async Task<bool> Save() {
            return await _messenger.SaveUserAsync(this);
        }

        public void SendMessage(string text, ButtonLayout layout) {
            SendMessageAsync(text, layout).RunSynchronously();
        }

        public async Task SendMessageAsync(string text, ButtonLayout layout) {
            await _messenger.SendMessageAsync(new Message() {
                Text = text,
                Recipient = this,
                Layout = layout
            });
        }
    }
}