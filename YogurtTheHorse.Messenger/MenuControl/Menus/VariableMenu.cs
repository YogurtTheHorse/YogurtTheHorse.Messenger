using System;

using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.MenuControl.Menus {
	public class VariableMenu<TVariable> : IUserMenu {
		protected virtual string StartMessage { get; }

		protected Func<string, TVariable> Parse { get; }
		protected Action<TVariable, Message, UserData> OnVariableParsed { get; }
		protected ButtonLayout Layout { get; }
		protected Action<Exception, Message, UserData> OnParseError { get; }

		public virtual string MenuName { get; }

		internal VariableMenu(
				string startMessage,
				Func<string, TVariable> parse,
				Action<TVariable, Message, UserData> onParsed,
				ButtonLayout layout,
				Action<Exception, Message, UserData> onParseError=null) {

			MenuName = $"{typeof(TVariable).GetType()}Menu";

			StartMessage = startMessage ?? throw new ArgumentNullException(nameof(startMessage));
			Parse = parse ?? throw new ArgumentNullException(nameof(parse));
			OnVariableParsed = onParsed ?? throw new ArgumentNullException(nameof(onParsed));
			Layout = layout ?? throw new ArgumentNullException(nameof(layout));

			OnParseError = onParseError;
		}

		public virtual void OnMessage(Message message, UserData userData) {
			try {
				TVariable parsed = Parse(message.Text);

				OnVariableParsed(parsed, message, userData);
			} catch (Exception ex) {
				OnParseError?.Invoke(ex, message, userData);
			}
		}

		public virtual void Open(User user, UserData userData, object sender) {
			user.SendMessage(StartMessage, Layout);
		}

		public virtual void Close(User user, UserData userData, object sender) { }
	}
}
