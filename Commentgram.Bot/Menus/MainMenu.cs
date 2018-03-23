using System;

using Commentgram.Bot.Layouts;
using YogurtTheHorse.Messenger;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Menus {
	public class MainMenu : SimpleUserMenu {
		protected override string StartMessage => "{MainMenu.StartMessage}";

		public override string MenuName => "MainMenu";

		public MainMenu(MenuController menuController) : base(menuController) {
			Layout = new MainMenuLayout();
		}
	}
}