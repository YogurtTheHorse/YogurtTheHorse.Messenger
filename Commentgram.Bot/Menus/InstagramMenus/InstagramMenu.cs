using Commentgram.Bot.DataTypes;
using System;
using System.Linq;
using YogurtTheHorse.Messenger.Database;
using YogurtTheHorse.Messenger.Localizations;
using YogurtTheHorse.Messenger.MenuControl.Buttons;
using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace Commentgram.Bot.Menus.InstagramMenus {
	internal class InstagramMenu : SimpleUserMenu {
		protected override string StartMessage => "{MainMenu.StartMessage}";

		public InstagramMenu() {
			IUserMenu setCommentsCount = Builders.GetVariableMenuBuilder<uint>().
				StartMessage("{InstagramMenu.SpecifyCount}").
				OnParsed((count, message, userdata) => {
					IDatabaseDriver database = message.Controller.Messenger.Database;
					database.InsertToCollection("links", new Link(userdata.UserID, userdata.LinkToAdd, count));
					message.Controller.Back(message.Recipient, userdata);

					// TODO: Take money ud.Amount
				}).
				Build();

			IUserMenu sendLinkMenu = Builders.GetVariableMenuBuilder<string, CommentgramUserData>().
				StartMessage("{InstagramMenu.SpecifyLink}").
				OnParsed((s, m, ud) => {
					ud.LinkToAdd = s;
					m.Controller.Switch(m.Recipient, ud, setCommentsCount.MenuName);
				}).
				Build();
				
			Layout = Builders.LayoutBuilder.
					AddButton(Builders.ButtonInfoBuilder.
						Text("{InstagramMenu.SendLink}").
						NavigateTo(sendLinkMenu.MenuName)).

					AddButton(Builders.ButtonInfoBuilder.
						Text("{InstagramMenu.GetLinks}").
						Callback(ShowLinks)).

				NextRow().
					AddButton(ButtonInfoBuilder.BackButton);
		}

		private void ShowLinks(object sender, ButtonActionEventArgs args) {
			var links = args.Database.GetQueryable<Link>("links").
				Where(l => l.AuthorID == args.User.UserID);

			string formatFunc(Link link) => LocaleManager.Format("InstaLinkFormat", args.User.LanguageCode,
								link.Path,
								link.CommentsCount,
								link.GetCommentsPublished(args.Database));

			string message = string.Join("\n\n", links.Select(formatFunc));
			args.User.SendMessage(message);
		}
	}
}
