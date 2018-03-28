using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus.AccountMenus {
    public class AccountMenu : SimpleUserMenu {
        protected override string StartMessage => "{AccountMenu.StartMessage}";

		public AccountMenu() {
			Layout = Builders.LayoutBuilder.
					AddButton(new ButtonInfoBuilder().
						NavigateTo<WalletMenu>().
						Text("{AccountMenu.WalletNumberMenu}")).

					AddButton(new ButtonInfoBuilder().
						NavigateTo<MoneyMenu>().
						Text("{AccountMenu.MoneyMenu}")).

				NextRow().
					AddButton(ButtonInfoBuilder.BackButton);
        }

    }
}