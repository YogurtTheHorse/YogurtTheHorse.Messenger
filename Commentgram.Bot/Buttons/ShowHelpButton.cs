
using System;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Buttons {
	public class ShowHelpButton : ButtonInfo {
		public ShowHelpButton() {
			Text = "{Info.Help}";
			ButtonType = EButtonType.Usual;
		}
	}
}
