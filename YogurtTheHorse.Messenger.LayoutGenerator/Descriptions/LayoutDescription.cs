using System;
using System.Collections.Generic;

namespace YogurtTheHorse.Messenger.LayoutGenerator.Descriptions {
    public class LayoutDescription {
        public enum ELayoutType {
            Inline,
            Usual,

            Other
        }

        public string Name { get; private set; }
        public bool ResizeKeyboard { get; private set; }
        public bool OneTimeKeyboard { get; private set; }

        public ELayoutType LayoutType { get; private set; }
        public List<List<ButtonDescription>> Buttons { get; private set; }

        public LayoutDescription() {
            Name = null;
            LayoutType = ELayoutType.Other;
            Buttons = new List<List<ButtonDescription>>();
        }

        public LayoutDescription(Generator generator, string name, dynamic value) {
            Name = name;

            string layoutTypeString = value.type ?? 
                throw new KeyNotFoundException($"Not found layout type for \"{name}\" layout.");

            if (!Enum.TryParse(layoutTypeString, true, out ELayoutType layoutType)) {
                throw new ArgumentException($"Unknown layout type \"{layoutTypeString}\" for \"{name}\" layout.");
            }

            ResizeKeyboard = value.resize ?? false;
            OneTimeKeyboard = value.one_time ?? false;

            Buttons = GetButtons(generator, value.orientation ?? "none", value.buttons);
        }

        private List<List<ButtonDescription>> GetButtons(Generator generator, string orientation, dynamic buttons) {
            var result = new List<List<ButtonDescription>>();

            if (buttons == null) {
                return result;
            }

            if (orientation == "horizontal") {
                result.Add(new List<ButtonDescription>());
            }

            foreach (var btnRow in buttons) {
                switch (orientation.ToLower()) {
                    case "vertical":
                        result.Add(new List<ButtonDescription>() { generator.GetButtonDescription(btnRow) });
                        break;

                    case "horizontal":
                        result[0].Add(generator.GetButtonDescription(btnRow));
                        break;

                    default:
                        result.Add(new List<ButtonDescription>());
                        foreach (var btn in btnRow) {
                            result[result.Count - 1].Add(generator.GetButtonDescription(btn));
                        }
                        break;
                }
            }

            return result;
        }
    }
}
