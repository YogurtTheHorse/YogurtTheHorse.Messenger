using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace YogurtTheHorse.Messenger.MenuControl {
	public static class VariableMenuCreator<TUserData, TVariable> where TUserData : UserData {
		private static Dictionary<string, VariableUserMenu<TUserData, TVariable>> _registeredMenus = new Dictionary<string, VariableUserMenu<TUserData, TVariable>>();

		public static VariableUserMenu<TUserData, TVariable> CreateVariableMenu(
				MenuController controller,
				Expression<Func<TUserData, TVariable>> memberExpression,
				Func<string, TVariable> parse,
				Action<Message, UserData> onSet) {
			var menu = new VariableUserMenu<TUserData, TVariable>(controller, memberExpression, parse, onSet);

			if (!_registeredMenus.ContainsKey(menu.MenuName)) {
				controller.RegisterMenuInstance(menu);
				_registeredMenus[menu.MenuName] = menu;
			}

			return menu;
		}

		public static VariableUserMenu<TUserData, TVariable> CreateVariableMenu(
				MenuController controller,
				Expression<Func<TUserData, TVariable>> memberExpression,
				Action<Message, UserData> onSet) {
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(TVariable));


			if (!(converter is null) && converter.CanConvertFrom(typeof(string))) {
				return CreateVariableMenu(controller, memberExpression, s => (TVariable)converter.ConvertFromString(s), onSet);
			} else {
				throw new NotSupportedException($"Can't convert {typeof(TVariable)} from string with TypeConverter. You should write own parse function");
			}
		}

		public static VariableUserMenu<TUserData, TVariable> CreateVariableMenu(
				MenuController controller,
				Expression<Func<TUserData, TVariable>> memberExpression) {
			
			return CreateVariableMenu(controller, memberExpression, (m, ud) => controller.Back(m.Recipient, ud));
		}
	}
}
