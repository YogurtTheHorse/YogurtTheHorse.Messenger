using System;
using System.Collections.Concurrent;

using YogurtTheHorse.Messenger.Database;

using NLog;

namespace YogurtTheHorse.Messenger.MenuControl {
	public class MenuController {
		private static Logger _logger = LogManager.GetLogger("MenuControllger");

		private IDatabaseDriver _database => Messenger.Database;
		private ConcurrentDictionary<string, IUserMenu> _menus;

		private Func<string, UserData> _generateUserData;

		public IMessenger Messenger { get; }


		public MenuController(IMessenger messenger) : this(messenger, (s) => new UserData(s)) { }

		public MenuController(IMessenger messenger, Func<string, UserData> generateUserData) {
			_generateUserData = generateUserData;
			Messenger = messenger;

			_menus = new ConcurrentDictionary<string, IUserMenu>();

			Messenger.OnIncomingMessage += OnMessage;
		}


		public void OpenMenu(User user, UserData userData, string menuName) {
			if (!_menus.ContainsKey(menuName)) {
				_logger.Error($"Tried to open unexisting menu: {menuName}");
				return;
			}
			userData.MenuStack.Push(menuName);
			_menus[menuName].Open(user, userData, this);
		}

		public void Back(User user, UserData userData) {
			if (userData.MenuStack.Count <= 1) {
				throw new InvalidOperationException("No menus to back");
			}

			_menus[userData.MenuStack.Pop()].Close(user, userData, this);
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
			UserData userData = _database.GetUserData(message.Recipient.UserID) ?? _generateUserData(message.Recipient.UserID);

			string menuName = userData.MenuStack.Peek();

			if (_menus.ContainsKey(menuName)) {
				_menus[menuName].OnMessage(message, userData);
				_database.SaveUserData(userData);
			} else {
				throw new InvalidOperationException($"No such menu registered: {menuName}");
			}
		}
	}
}
