using Commentgram.Bot.Buttons;

using Commentgram.Bot.Menus.AccountMenus;
using Commentgram.Bot.Menus.InstagramMenus;

using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus {
	public class MainMenu : SimpleUserMenu {
		protected override string StartMessage => "{MainMenu.StartMessage}";

		public override string MenuName => "MainMenu";

		public MainMenu() {
			Layout = Builders.LayoutBuilder.
					AddButton(Builders.ButtonInfoBuilder.
						Text("{MainMenu.Comment}").
						NavigateTo("CommentMenu")).

					AddButton(Builders.ButtonInfoBuilder.
						Text("{MainMenu.Instagram}").
						NavigateTo<InstagramMenu>()).

				NextRow().
					AddButton(Builders.ButtonInfoBuilder.
						Text("{MainMenu.Account}").
						NavigateTo<AccountMenu>()).

					AddButton(new ShowHelpButton());
		}
	}
}