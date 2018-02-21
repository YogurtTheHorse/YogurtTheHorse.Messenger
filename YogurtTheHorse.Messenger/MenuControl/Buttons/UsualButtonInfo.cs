using System;
using System.Collections.Generic;
using System.Text;

namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
    public class UsualButtonInfo : ButtonInfo {
        public override EButtonType ButtonType => EButtonType.Usual;

        public UsualButtonInfo(string text) {
            Text = text;
        }

        public override void Action<TUserData>(object sender, ButtonActionEventArgs<TUserData> e) { }
    }
}
