using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus.AccountMenus {
    public class AccountMenu : SimpleUserMenu {
        protected override string StartMessage => "{AccountMenu.StartMessage}";

		public AccountMenu() {
			IUserMenu followingMenu = Builders.GetVariableMenuBuilder<FollowingType, CommentgramUserData>().
				SetMember(ud => ud.FollowingType).
				StartMessage("{AccountMenu.SpecifyFollowingType}").
				Build();
				
			Layout = Builders.LayoutBuilder.
					AddButton(Builders.ButtonInfoBuilder.
						NavigateTo<WalletMenu>().
						Text("{AccountMenu.WalletNumberMenu}")).

					AddButton(Builders.ButtonInfoBuilder.
						NavigateTo<MoneyMenu>().
						Text("{AccountMenu.MoneyMenu}")).

				NextRow().

					AddButton(Builders.ButtonInfoBuilder.
						NavigateTo<InstagramCreditanalsMenu>().
						Text("{AccountMenu.InstagramSettings}")).

					AddButton(followingMenu.
						NavigateTo().
						Text("{AccountMenu.FollowingType}")).

				NextRow().
					AddButton(ButtonInfoBuilder.BackButton);
        }

    }
}