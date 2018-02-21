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

        public ButtonDescription(dynamic d) : this(null, (DynamicObject)d) { }

        public ButtonDescription(string name, dynamic d) {
            Name = name ?? d.name;

            string buttonTypeString = d.button_type ?? (d.data == null ? "input" : "navigate");
            
            if (!Enum.TryParse(buttonTypeString, true, out EButtonType buttonType) || buttonType == EButtonType.Incorrect) {
                throw new InvalidOperationException($"Unsupported button type: {buttonTypeString}");
            }

            ButtonType = buttonType;

            Text = d.text ?? "";
            Data = d.data;
        }
    }
}