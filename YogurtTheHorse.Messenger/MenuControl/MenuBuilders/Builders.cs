using System;
using System.Collections.Generic;
using System.Text;

namespace YogurtTheHorse.Messenger.MenuControl.MenuBuilders {
	public static class Builders {
		public static MenuBuilder MenuBuilder => new MenuBuilder();
		public static MenuControllerBuilder MenuControllerBuilder => new MenuControllerBuilder();
		public static LayoutBuilder LayoutBuilder => new LayoutBuilder();
		public static ButtonInfoBuilder ButtonInfoBuilder => new ButtonInfoBuilder();

		public static VariableMenuBuilder<T> GetVariableMenuBuilder<T>() => new VariableMenuBuilder<T>();
		public static VariableMenuBuilder<T, U> GetVariableMenuBuilder<T, U>() where U : UserData => new VariableMenuBuilder<T, U>();
	}
}
