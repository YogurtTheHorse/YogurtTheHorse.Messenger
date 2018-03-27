using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;
using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus.AccountMenus {
	public class WalletMenu : SimpleUserMenu {
		protected override string StartMessage => "{AccountMenu.WalletMenu.StartMessage}";

		public WalletMenu(MenuController menuController) : base() {
			Layout = new ButtonInfo[][] {
				new ButtonInfo[] {
					new VariableMenuBuilder<string, CommentgramUserData>().
						SetMember(u => u.YandexWallet).
						Build().
						NavigateTo().
						Text("{AccountMenu.WalletMenu.ChangeWallet}"),

					new ButtonInfoBuilder().
						Text("{AccountMenu.WalletMenu.DeleteWallet}").
						Callback((s, e) => {
							(e.UserData as CommentgramUserData).YandexWallet = null;
							Open(e.User, e.UserData, this);
						}).
						HideCondition((u, d) => (d as CommentgramUserData).YandexWallet is null)
				},
				new [] {
					new ButtonInfoBuilder().BackButton().ToButton()
				}
			};
		}
	}
}