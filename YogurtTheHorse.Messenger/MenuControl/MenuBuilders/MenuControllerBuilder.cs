using System;
using System.Collections.Generic;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace YogurtTheHorse.Messenger.MenuControl.MenuBuilders {
	public sealed class MenuControllerBuilder {
		private List<Action<MenuController>> _menusAddingActions;
		private IMessenger _messenger;
		private Func<string, UserData> _generateUserData;

		public MenuControllerBuilder() {
			_menusAddingActions = new List<Action<MenuController>>();
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

		public MenuControllerBuilder AddMenu<TUserMenu>(TUserMenu userMenu) where TUserMenu : IUserMenu {
			_menusAddingActions.Add((c) => c.RegisterMenuInstance(userMenu));
			return this;
		}

		public MenuControllerBuilder AddMenu<TUserMenu>() where TUserMenu : IUserMenu, new() {
			_menusAddingActions.Add((c) => c.RegisterMenuClass<TUserMenu>());
			return this;
		}

		public MenuController Build() {
			var controller = new MenuController(_messenger, _generateUserData);

			foreach (Action<MenuController> menuRegistration in _menusAddingActions) {
				menuRegistration(controller);
			}

			return controller;
		}

		public static implicit operator MenuController(MenuControllerBuilder builder) {
			return builder.Build();
		}
	}
}
