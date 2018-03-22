using System;
using System.Dynamic;

namespace YogurtTheHorse.Messenger.LayoutGenerator.Descriptions {
    public struct ButtonDescription {
        public enum EButtonType {
            Incorrect,
            
            Inline,

            Navigate,
            Input,
            Other
        }

        public EButtonType ButtonType { get; }
        public string Text { get; }
        public object Data { get; }
        public string Name { get; }
		public string Action { get; }

        public ButtonDescription(dynamic d) : this(null, (DynamicObject)d) { }

        public ButtonDescription(string name, dynamic d) {
            Name = name ?? d.name;

            string buttonTypeString = d.button_type ?? (d.menu_to_open != null ? "navigate" : "input");
            
            if (!Enum.TryParse(buttonTypeString, true, out EButtonType buttonType) || buttonType == EButtonType.Incorrect) {
                throw new InvalidOperationException($"Unsupported button type: {buttonTypeString}");
            }

			if (buttonType == EButtonType.Navigate && d.menu_to_open == null) {
				throw new InvalidOperationException($"Navigation button must has `menu_to_open` field. {name} doesn't have it.");
			}

            ButtonType = buttonType;

            Text = d.text ?? "";
            Data = buttonType == EButtonType.Navigate ? d.menu_to_open : d.data;
			Action = d.action;
        }
    }
}