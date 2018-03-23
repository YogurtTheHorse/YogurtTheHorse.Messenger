using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
    public class ButtonLayout {
        protected List<List<ButtonInfo>> Buttons;

        public virtual EButtonType LayoutType { get; }
        public virtual bool ResizeKeyboard { get; }
        public virtual bool OneTimeKeyboard { get; }

		#region Constructors
		public ButtonLayout() : this(false, false) { }

        public ButtonLayout(bool resizeKeyboard, bool oneTimeKeyboard) :
            this(resizeKeyboard, oneTimeKeyboard, EButtonType.Other) { }

        public ButtonLayout(bool resizeKeyboard, bool oneTimeKeyboard, EButtonType layoutType) {
            ResizeKeyboard = resizeKeyboard;
            OneTimeKeyboard = oneTimeKeyboard;

            Buttons = new List<List<ButtonInfo>>();
			LayoutType = layoutType;
        }

        public ButtonLayout(ICollection<ButtonInfo> buttons, bool isVertical, bool resizeKeyboard, bool oneTimeKeyboard) :
            this(resizeKeyboard, oneTimeKeyboard) {

            if (isVertical) {
                LayoutType = buttons?.GetEnumerator().Current?.ButtonType ?? EButtonType.Other;
                Buttons.Add(new List<ButtonInfo>(buttons));
            } else {
                foreach (var b in buttons) {
                    LayoutType = b.ButtonType;
                    Buttons.Add(new[] { b }.ToList());
                }
            }

            CheckButtonsType();
        }

        public ButtonLayout(ICollection<ICollection<ButtonInfo>> buttons) : this(buttons, false, false) { }

        public ButtonLayout(ICollection<ICollection<ButtonInfo>> buttons, bool resizeKeyboard, bool oneTimeKeyboard) : this(resizeKeyboard, oneTimeKeyboard) {
            Buttons = new List<List<ButtonInfo>>();
			Buttons.AddRange(buttons.Select(row => new List<ButtonInfo>(row)));

            CheckButtonsType();
		}
		#endregion

		private void CheckButtonsType() { }

		#region Enumerators
		public IEnumerable<ButtonInfo> GetButtons() {
            foreach (var lb in Buttons) {
                foreach (var bi in lb) {
                    yield return bi;
                }
            }
        }

        public IEnumerable<ButtonInfo> GetUsualButtons() {
            foreach (var btn in GetButtons()) {
				if (btn.ButtonType == EButtonType.Usual) {
					yield return btn;
				}
            }
        }
		#endregion
	}
}