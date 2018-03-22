 
using System; 
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace Commentgram.Bot.Buttons {
	public class ShowHelpButton : UsualButtonInfo {
        public ShowHelpButton() : base("{Info.Help}") { }
       public override void Action<TUserData>(object sender, ButtonActionEventArgs<TUserData> e) {
            /* show help */;
        } 
    }
}
