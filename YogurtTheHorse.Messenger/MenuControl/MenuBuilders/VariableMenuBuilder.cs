using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using YogurtTheHorse.Messenger.MenuControl.Buttons;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace YogurtTheHorse.Messenger.MenuControl.MenuBuilders {
	public class VariableMenuBuilder<TVariable> {
		protected internal static Dictionary<string, VariableMenu<TVariable>> _registeredMenus = new Dictionary<string, VariableMenu<TVariable>>();

		protected internal string _startMessage;
		protected internal Func<string, TVariable> _parse;
		protected internal Action<TVariable, Message, UserData> _onParsed;
		protected internal ButtonLayout _layout = new ButtonLayout();
		protected internal Action<Exception, Message, UserData> _onParseError = null;
		protected internal string _menuName = typeof(TVariable).FullName + "Menu";

		public VariableMenuBuilder() {
			_startMessage = "Specify variable:";
			_onParsed = (v, m, ud) => m.Controller.Back(m.Recipient, ud);

			TypeConverter converter = TypeDescriptor.GetConverter(typeof(TVariable));

			if (!(converter is null) && converter.CanConvertFrom(typeof(string))) {
				_parse = s => (TVariable)converter.ConvertFromString(s);
			} else {
				_parse = null;
			}
		}

		public VariableMenuBuilder<TVariable> MenuName(string text) {
			_menuName = text;
			return this;
		}

		public VariableMenuBuilder<TVariable> StartMessage(string text) {
			_startMessage = text;
			return this;
		}

		public VariableMenuBuilder<TVariable> Parse(Func<string, TVariable> parse) {
			_parse = parse;
			return this;
		}

		public VariableMenuBuilder<TVariable> OnParsed(Action<TVariable, Message, UserData> parsed) {
			_onParsed = parsed;
			return this;
		}

		public VariableMenuBuilder<TVariable> Layout(ButtonLayout layout) {
			_layout = layout;
			return this;
		}

		public VariableMenuBuilder<TVariable> OnParseError(Action<Exception, Message, UserData> onParseError) {
			_onParseError = onParseError;
			return this;
		}

		public VariableMenuBuilder<TVariable, TUserData> SetMember<TUserData>(Expression<Func<TUserData, TVariable>> expression) where TUserData : UserData {
			throw new NotImplementedException();
			//return ((VariableMenuBuilder<TVariable, TUserData>)this).SetMember(expression);
		}

		public VariableMenu<TVariable> Build() {
			var menu = ToMenu();

			if (!_registeredMenus.ContainsKey(menu.MenuName)) {
				_registeredMenus[menu.MenuName] = menu;
				MenuController.GlobalMenus[menu.MenuName] = menu;
			}

			return menu;
		}

		public virtual VariableMenu<TVariable> ToMenu() {
			return new VariableMenu<TVariable>(
					_menuName,
					_startMessage,
					_parse,
					_onParsed,
					_layout,
					_onParseError);
		}

		public static implicit operator VariableMenu<TVariable>(VariableMenuBuilder<TVariable> builder) {
			return builder.Build();
		}
	}

	public sealed class VariableMenuBuilder<TVariable, TUserData> : VariableMenuBuilder<TVariable> where TUserData : UserData {
		private MemberExpression _memberExpression;

		public VariableMenuBuilder<TVariable, TUserData> SetMember(Expression<Func<TUserData, TVariable>> expression) {
			_memberExpression = expression.Body as MemberExpression;
			_menuName = typeof(TUserData).FullName + "_" + _memberExpression.Member.Name + "_Menu";

			if (_memberExpression is null) {
				throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");
			}

			return this;
		}

		public override VariableMenu<TVariable> ToMenu() {
			return new VariableMenu<TVariable>(
				_menuName,
				_startMessage,
				_parse,
				OnParsed,
				_layout,
				_onParseError);
		}

		private void OnParsed(TVariable parsed, Message message, UserData userData) {
			if (_memberExpression.Member is PropertyInfo propertyInfo) {
				propertyInfo.SetValue(userData, parsed);
			} else {
				(_memberExpression.Member as FieldInfo).SetValue(userData, parsed);
			}

			_onParsed(parsed, message, userData);
		}
	}
}
