using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;
using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus.AccountMenus {
	public class MoneyMenu : SimpleUserMenu {
		protected override string StartMessage => "{AccountMenu.WalletMenu.StartMessage}";

		public MoneyMenu(MenuController menuController) {
			Layout = new LayoutBuilder().
					AddButton(new VariableMenuBuilder<string, CommentgramUserData>().
						SetMember(u => u.YandexWallet).
						Build().
						NavigateTo().
						Text("{AccountMenu.WalletMenu.ChangeWallet}")).

					AddButton(new ButtonInfoBuilder().
						Text("{AccountMenu.WalletMenu.DeleteWallet}").
						Callback((s, e) => {
							(e.UserData as CommentgramUserData).YandexWallet = null;
							Open(e.User, e.UserData, this);
						}).
						HideCondition((u, d) => (d as CommentgramUserData).YandexWallet is null)).
				NextRow().
					AddButton(ButtonInfoBuilder.BackButton);
		}
	}
}