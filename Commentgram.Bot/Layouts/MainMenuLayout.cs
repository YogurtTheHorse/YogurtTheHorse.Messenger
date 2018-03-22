using Commentgram.Bot.Buttons;
using System.Collections.Generic;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Layouts {
    public class MainMenuLayout : ButtonLayout {
        public MainMenuLayout() : base(false, true, EButtonType.Usual) {
            Buttons = new List<List<ButtonInfo>>();

            Buttons.Add(new List<ButtonInfo>());

            Buttons[0].Add(("{MainMenu.Comment}", "CommentMenu"));
            Buttons[0].Add(("{MainMenu.Instagram}", "InstagramMenu"));
            Buttons.Add(new List<ButtonInfo>());

            Buttons[1].Add(("{MainMenu.Account}", "AccountMenu"));
            Buttons[1].Add(new ShowHelpButton());
        }
    }
}

