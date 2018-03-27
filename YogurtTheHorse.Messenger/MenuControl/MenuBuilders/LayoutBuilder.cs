using System;
using System.Collections.Generic;
using System.Text;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.MenuControl.MenuBuilders {
	public sealed class LayoutBuilder {
		private List<List<ButtonInfo>> _buttons;

		private EButtonType _layoutType = EButtonType.Other;
		private bool _resizeKeyboard;
		private bool _oneTimeKeyboard;

		public LayoutBuilder() {
			_buttons = new List<List<ButtonInfo>> {
				new List<ButtonInfo>()
			};

			_resizeKeyboard = ButtonLayout.DefaultButtonLayoutValues.ResizeKeyboard;
			_oneTimeKeyboard = ButtonLayout.DefaultButtonLayoutValues.OneTimeKeyboard;
		}

		public LayoutBuilder Reize(bool resize) {
			_resizeKeyboard = resize;
			return this;
		}

		public LayoutBuilder OneTime(bool oneTime) {
			_oneTimeKeyboard = oneTime;
			return this;
		}

		public LayoutBuilder LayoutType(EButtonType layoutType) {
			_layoutType = layoutType;
			return this;
		}

		public LayoutBuilder AddButton(ButtonInfo buttonInfo) {
			if (buttonInfo.ButtonType != _layoutType) {
				throw new InvalidOperationException($"ButtonInfo.ButtonType doesn't equal to layout type.");
			}

			_buttons[_buttons.Count - 1].Add(buttonInfo);

			return this;
		}

		public LayoutBuilder NextRow() {
			_buttons.Add(new List<ButtonInfo>());

			return this;
		}

		public ButtonLayout Build() {
			return new ButtonLayout((ICollection<ICollection<ButtonInfo>>)_buttons, _resizeKeyboard, _oneTimeKeyboard);
		}

		public static implicit operator ButtonLayout(LayoutBuilder builder) {
			return builder.Build();
		}
	}
}
