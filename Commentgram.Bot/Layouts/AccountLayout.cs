using Commentgram.Bot.Buttons;
using System.Collections.Generic;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Layouts {
    public class AccountLayout : ButtonLayout {
        public AccountLayout() : base(false, true, EButtonType.Inline) {
            Buttons = new List<List<ButtonInfo>>();

            Buttons.Add(new List<ButtonInfo>());

            Buttons[0].Add(("{AccountMenu.WalletNumber}", "WalletMenu"));
            Buttons.Add(new List<ButtonInfo>());

            Buttons[1].Add(("{AccountMenu.Money}", "MoneyMenu"));
            Buttons.Add(new List<ButtonInfo>());

            Buttons[2].Add(new FollowingButton());
        }
    }
}

