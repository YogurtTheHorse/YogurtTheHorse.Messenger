using YogurtTheHorse.Messenger;
using YogurtTheHorse.Messenger.Localizations;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus.AccountMenus {
	public class MoneyMenu : SimpleUserMenu {
		public MoneyMenu() {
			Layout = new LayoutBuilder().
					AddButton(new VariableMenuBuilder<string, CommentgramUserData>().
							SetMember(u => u.YandexWallet).
							Build().
						NavigateTo().
						Text("{AccountMenu.MoneyMenu.Deposit}")).

					AddButton(new ButtonInfoBuilder().
						Text("{AccountMenu.MoneyMenu.Withdraw}").
						HideCondition((u, d) => (d as CommentgramUserData).YandexWallet is null)).

				NextRow().
					AddButton(ButtonInfoBuilder.BackButton);
		}

		public override void Open(User user, UserData userData, object sender) {
			string msg = LocaleManager.Format(
				"AccountMenu.MoneyMenu.StartMessage", 
				userData.Locale, 
				(userData as CommentgramUserData).Amount);

			user.SendMessage(msg, Layout);
		}
	}
}