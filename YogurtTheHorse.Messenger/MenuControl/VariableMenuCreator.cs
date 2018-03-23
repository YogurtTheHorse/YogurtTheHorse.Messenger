using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace YogurtTheHorse.Messenger.MenuControl {
	public static class VariableMenuCreator<TUserData, TVariable> where TUserData : UserData {
		public static VariableUserMenu<TUserData, TVariable> CreateVariableMenu(
				MenuController controller,
				Expression<Func<TUserData, TVariable>> memberExpression,
				Func<string, TVariable> parse) {
			var menu = new VariableUserMenu<TUserData, TVariable>(controller, memberExpression, parse);

			try {
				controller.RegisterMenuInstance(menu);
			} catch (ArgumentException) { }

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
