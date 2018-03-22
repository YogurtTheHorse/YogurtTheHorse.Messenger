 
using System; 
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Buttons {
	public class FollowingButton : UsualButtonInfo {
        public FollowingButton() : base("{AccountMenu.Following}") { }
       public override void Action<TUserData>(object sender, ButtonActionEventArgs<TUserData> e) {
            /* update following type */;
        } 
    }
}
