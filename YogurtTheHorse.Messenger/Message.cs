using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger {
    public struct Message {
		public MenuController Controller { get; set; }

		public MessageType MessageType { get; set; }
        public string Text { get; set; }
        public User Recipient { get; set; }
        public ImageInfo ImageInfo { get; set; }
        public ButtonLayout Layout { get; set; }
	}

    public enum MessageType {
        Text,
        Image,
        Other
    }
}