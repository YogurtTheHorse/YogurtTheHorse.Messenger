using Commentgram.Bot.Buttons;
using Commentgram.Bot.Menus.AccountMenus;
using System.Collections.Generic;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Layouts {
	public class MainMenuLayout : ButtonLayout {
		public MainMenuLayout() : base(false, true, EButtonType.Usual) {
			Buttons = new List<List<ButtonInfo>>();

			Buttons.Add(new List<ButtonInfo>());

			Buttons[0].Add(new ButtonInfoBuilder().Text("{MainMenu.Comment}").NavigateTo("CommentMenu"));
			Buttons[0].Add(new ButtonInfoBuilder().Text("{MainMenu.Instagram}").NavigateTo("InstagramMenu"));
			Buttons.Add(new List<ButtonInfo>());

			Buttons[1].Add(new ButtonInfoBuilder().Text("{MainMenu.Account}").NavigateTo<AccountMenu>());
			Buttons[1].Add(new ShowHelpButton());
		}
	}
}

