using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace YogurtTheHorse.Messenger.MenuControl {
	public static class VariableMenuCreator {
		public static VariableUserMenu<TUserData, TVariable> CreateVariableMenu<TUserData, TVariable>(
				MenuController<TUserData> controller,
				Expression<Func<TUserData, TVariable>> memberExpression,
				Func<string, TVariable> parse) where TUserData : class, IUserData {
			var menu = new VariableUserMenu<TUserData, TVariable>(controller, memberExpression, parse);
			try {
				controller.RegisterMenuInstance(menu);
			} catch (ArgumentException e) {
				throw new ArgumentException("Variable menu for that member alread registered", e);
			}
			return menu;
		}

		public static VariableUserMenu<TUserData, string> CreateVariableMenu<TUserData, TVariable>(
				MenuController<TUserData> controller,
				Expression<Func<TUserData, string>> memberExpression) where TUserData : class, IUserData {
			return CreateVariableMenu(controller, memberExpression, s => s);
		}

		public static VariableUserMenu<TUserData, TVariable> CreateVariableMenu<TUserData, TVariable>(
				MenuController<TUserData> controller,
				Expression<Func<TUserData, TVariable>>
				memberExpression) where TUserData : class, IUserData {
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(TVariable));


			if (!(converter is null) && converter.CanConvertFrom(typeof(string))) {
				return CreateVariableMenu(controller, memberExpression, s => (TVariable)converter.ConvertFromString(s));
			} else {
				throw new NotSupportedException($"Can't convert {typeof(TVariable)} from string with TypeConverter. You should write own parse function");
			}
		}
	}
}
