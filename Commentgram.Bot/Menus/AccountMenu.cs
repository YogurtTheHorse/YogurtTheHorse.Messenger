using System;

using Commentgram.Bot.Layouts;
using YogurtTheHorse.Messenger;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Menus {
    public class AccountMenu : SimpleUserMenu<CommentgramUserData> {
        protected override string StartMessage => "{AccountMenu.StartMessage}";
        protected override ButtonLayout Layout { get; }

        public override string MenuName => "AccountMenu";

        public AccountMenu(MenuController<CommentgramUserData> menuController) : base(menuController) {
            Layout = new AccountLayout();
        }

    }
}