using System;

using Commentgram.Bot.Layouts;

using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Menus.AccountMenus {
    public class AccountMenu : SimpleUserMenu {
        protected override string StartMessage => "{AccountMenu.StartMessage}";

		public AccountMenu(MenuController menuController) : base(menuController) {
			Layout = new ButtonInfo[][] {
				new [] {
					new ButtonInfoBuilder().NavigateTo<WalletMenu>().Text("{AccountMenu.WalletNumberMenu}").ToButton()
				},
				new [] {
					new ButtonInfoBuilder().BackButton().ToButton()
				}
			};
        }

    }
}