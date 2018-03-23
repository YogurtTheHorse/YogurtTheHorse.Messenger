using System;
using System.Linq.Expressions;
using System.Reflection;

using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.MenuControl {
	public class VariableUserMenu<TUserData, TVariable> : SimpleUserMenu<TUserData> where TUserData : class, IUserData {
		protected bool IsProperty { get; }
		protected MemberExpression MemberExpression { get; }
		protected Func<string, TVariable> Parse { get; }

		protected override string StartMessage { get; }
		public override string MenuName => MemberExpression.Member.Name + "Menu";


		internal VariableUserMenu(MenuController<TUserData> menuController, Expression<Func<TUserData, TVariable>> memberExpression, Func<string, TVariable> parse)
		: base(menuController) {
			StartMessage = "Please specify variable:";
			Parse = parse;
			MemberExpression = memberExpression.Body as MemberExpression;
			if (memberExpression is null) {
				throw new ArgumentException($"Expression '{memberExpression}' refers to a method, not a property.");
			}

			IsProperty = MemberExpression.Member is PropertyInfo;
			Layout = new ButtonInfo[0][];
		}

		public override void OnMessage(Message message, TUserData userData) {
			TVariable parsed = Parse(message.Text);
			if (IsProperty) {
				(MemberExpression.Member as PropertyInfo).SetValue(userData, parsed);
			} else {
				(MemberExpression.Member as FieldInfo).SetValue(userData, parsed);
			}
		}
	}
}
