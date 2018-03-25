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
				Func<string, TVariable> parse) {
			var menu = new VariableUserMenu<TUserData, TVariable>(controller, memberExpression, parse);

			if (!_registeredMenus.ContainsKey(menu.MenuName)) {
				controller.RegisterMenuInstance(menu);
				_registeredMenus[menu.MenuName] = menu;
			}

			return menu;
		}

		public static VariableUserMenu<TUserData, TVariable> CreateVariableMenu(
				MenuController controller,
				Expression<Func<TUserData, TVariable>>
				memberExpression) {
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(TVariable));


			if (!(converter is null) && converter.CanConvertFrom(typeof(string))) {
				return CreateVariableMenu(controller, memberExpression, s => (TVariable)converter.ConvertFromString(s));
			} else {
				throw new NotSupportedException($"Can't convert {typeof(TVariable)} from string with TypeConverter. You should write own parse function");
			}
		}
	}
}
