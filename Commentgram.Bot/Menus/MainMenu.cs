using Commentgram.Bot.Layouts;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus {
	public class MainMenu : SimpleUserMenu {
		protected override string StartMessage => "{MainMenu.StartMessage}";

		public override string MenuName => "MainMenu";

		public MainMenu(MenuController menuController) {
			Layout = new MainMenuLayout();
		}
	}
}