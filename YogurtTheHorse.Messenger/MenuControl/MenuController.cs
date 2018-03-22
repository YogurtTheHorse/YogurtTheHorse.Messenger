using System;
using System.Collections.Concurrent;

using YogurtTheHorse.Messenger.Database;

using NLog;

namespace YogurtTheHorse.Messenger.MenuControl {
	public class MenuController<TUserData> where TUserData : IUserData {
		private static Logger _logger = LogManager.GetLogger("MenuControllger");

		private IDatabaseDriver _database => Messenger.Database;
		private ConcurrentDictionary<string, IUserMenu> _menus;

		public IMessenger Messenger;


		public MenuController(IMessenger messenger) {
			Messenger = messenger;

			_menus = new ConcurrentDictionary<string, IUserMenu>();

			_database.RegisterUserDataType<TUserData>();
		}

		public void SomeMethod(string name) => Console.WriteLine($"Hello, {name}");


		public void OpenMenu(User user, IUserData userData, string menuName) {
			if (!_menus.ContainsKey(menuName)) {
				_logger.Error($"Tried to open unexisting menu: {menuName}");
				return;
			}
			userData.MenuName = menuName;
			_menus[menuName].Open(user, userData, this);
		}

		public void RegisterMenuInstance<TUserMenu>(TUserMenu menu) where TUserMenu : IUserMenu {
			if (_menus.ContainsKey(menu.MenuName)) {
				throw new ArgumentException($"{menu.MenuName} already registered");
			}


			_database.RegisterUserMenuClass<TUserMenu>();
			_menus[menu.MenuName] = menu;
		}

		public void RegisterMenuClass<TUserMenu>() where TUserMenu : IUserMenu, new() {
			TUserMenu menu = new TUserMenu();

			RegisterMenuInstance(menu);
		}

		public void OnMessage(Message message) {
			IUserData userData = _database.GetUserData(message.Recipient.UserID) ?? default(TUserData);

			if (_menus.ContainsKey(userData.MenuName)) {
				_menus[userData.MenuName].OnMessage(message, userData);
			} else {
				throw new InvalidOperationException($"No such menu registered: {userData.MenuName}");
			}
		}
	}
}
