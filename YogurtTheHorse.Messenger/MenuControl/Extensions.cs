using System;
using System.Collections.Generic;
using System.Text;
using YogurtTheHorse.Messenger.MenuControl.MenuBuilders;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace YogurtTheHorse.Messenger.MenuControl {
	public static class Extensions {
		public static ButtonInfoBuilder NavigateTo(this IUserMenu menu) {
			return new ButtonInfoBuilder().NavigateTo(menu.MenuName);
		}
	}
}
