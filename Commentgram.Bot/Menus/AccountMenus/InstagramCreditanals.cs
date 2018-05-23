using System;
using YogurtTheHorse.Messenger;
using YogurtTheHorse.Messenger.Localizations;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus.AccountMenus {
	public class InstagramCreditanalsMenu : SimpleUserMenu {
		public InstagramCreditanalsMenu() {
			IUserMenu setPasswordMenu = Builders.GetVariableMenuBuilder<string, CommentgramUserData>().
				SetMember(u => u.InstagramPassword).
				StartMessage("{InstagramMenu.SpecifyPassword}").
				OnParsed((s, m, ud) => {
					(ud as CommentgramUserData).CheckInstagramAuth();
					m.Controller.Back(m.Recipient, ud);
				}).
				Build();

			IUserMenu setLoginMenu = Builders.GetVariableMenuBuilder<string, CommentgramUserData>().
				SetMember(u => u.InstagramLogin).
				StartMessage("{InstagramMenu.SpecifyLogin}").
				OnParsed((s, m, ud) => m.Controller.Switch(m.Recipient, ud, setPasswordMenu.MenuName)).
				Build();

			Layout = Builders.LayoutBuilder.
				AddButton(setLoginMenu.
					NavigateTo().
					Text("{InstagramMenu.Auth}").
					HideCondition((u, ud) => !((ud as CommentgramUserData).InstagramLogin is null)).
					ToButton()).
				AddButton(Builders.ButtonInfoBuilder.
					Text("{InstagramMenu.RemoveLogin}").
					HideCondition((u, ud) => ((ud as CommentgramUserData).InstagramLogin is null)).
					Callback((o, e) => {
						var userData = (e.UserData as CommentgramUserData);

						userData.InstagramLogin = null;
						userData.InstagramPassword = null;

						Open(e.User, e.UserData, this);
					}).
					ToButton()).

				AddButton(ButtonInfoBuilder.BackButton);
		}

		public override void Open(User user, UserData userData, object sender) {
			CommentgramUserData commentgramUserData = userData as CommentgramUserData;

			string message = LocaleManager.GetString(commentgramUserData.IsInstagramAuthinticated ? "InstagramAuthinticated" : "No auth", userData.Locale);
			
			user.SendMessage(message, Layout.GetButtons(user, userData));
		}
	}
}
