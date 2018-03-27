using System;

using Commentgram.Bot.Layouts;

using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;
using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus.AccountMenus {
    public class AccountMenu : SimpleUserMenu {
        protected override string StartMessage => "{AccountMenu.StartMessage}";

		public AccountMenu(MenuController menuController) : base() {
			Layout = new ButtonInfo[][] {
				new ButtonInfo[] {
					new ButtonInfoBuilder().NavigateTo<WalletMenu>().Text("{AccountMenu.WalletNumberMenu}"),
					new ButtonInfoBuilder().NavigateTo<MoneyMenu>().Text("{AccountMenu.MoneyMenu}")
				},
				new ButtonInfo[] {
					new ButtonInfoBuilder().BackButton()
				}
			};
        }

    }
}