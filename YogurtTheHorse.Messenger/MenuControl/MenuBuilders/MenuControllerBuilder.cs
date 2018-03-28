using System;
using System.Collections.Generic;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace YogurtTheHorse.Messenger.MenuControl.MenuBuilders {
	public sealed class MenuControllerBuilder {
		private List<IUserMenu> _menus;
		private IMessenger _messenger;
		private Func<string, UserData> _generateUserData;

		public MenuControllerBuilder() {
			_menus = new List<IUserMenu>();
			_generateUserData = (s) => new UserData(s);
		}

		public MenuControllerBuilder GenerateUserData(Func<string, UserData> func) {
			_generateUserData = func;
			return this;
		}

		public MenuControllerBuilder Messenger(IMessenger messenger) {
			_messenger = messenger;
			return this;
		}

		public MenuControllerBuilder AddMenu(IUserMenu userMenu) {
			_menus.Add(userMenu);
			return this;
		}

		public MenuController Build() {
			var controller = new MenuController(_messenger, _generateUserData);

			foreach (var menu in _menus) {
				controller.RegisterMenuInstance(menu);
			}

			return controller;
		}

		public static implicit operator MenuController(MenuControllerBuilder builder) {
			return builder.Build();
		}
	}
}
