using System;

using Commentgram.Bot.Layouts;
using YogurtTheHorse.Messenger;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Menus {
    public class MainMenu : SimpleUserMenu<CommentgramUserData> {
        protected override string StartMessage => "{MainMenu.StartMessage}";
        protected override ButtonLayout Layout { get; }

        public override string MenuName => "MainMenu";

        public MainMenu(MenuController<CommentgramUserData> menuController) : base(menuController) {
            Layout = new MainMenuLayout();
        }


    }
}