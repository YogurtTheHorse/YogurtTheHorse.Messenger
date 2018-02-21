using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YogurtTheHorse.Messenger.MenuControl.Buttons {
    public class ButtonLayout : IEnumerable<List<ButtonInfo>> {
        protected List<List<ButtonInfo>> Buttons;

        public virtual EButtonType LayoutType { get; }
        public virtual bool ResizeKeyboard { get; }
        public virtual bool OneTimeKeyboard { get; }

        public ButtonLayout() : this(false, false) { }

        public ButtonLayout(bool resizeKeyboard, bool oneTimeKeyboard) :
            this(resizeKeyboard, oneTimeKeyboard, EButtonType.Other) { }

        public ButtonLayout(bool resizeKeyboard, bool oneTimeKeyboard, EButtonType layoutType) {
            ResizeKeyboard = resizeKeyboard;
            OneTimeKeyboard = oneTimeKeyboard;

            Buttons = new List<List<ButtonInfo>>();
        }

        public ButtonLayout(IEnumerable<ButtonInfo> buttons, bool isVertical, bool resizeKeyboard, bool oneTimeKeyboard) :
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

        private void CheckButtonsType() {
        }

        public ButtonLayout(List<List<ButtonInfo>> buttons) : this(buttons, false, false) { }

        public ButtonLayout(List<List<ButtonInfo>> buttons, bool resizeKeyboard, bool oneTimeKeyboard) : this(resizeKeyboard, oneTimeKeyboard) {
            Buttons = buttons;

            CheckButtonsType();
        }

        public IEnumerable<ButtonInfo> GetButtons() {
            foreach (var lb in Buttons) {
                foreach (var bi in lb) {
                    yield return bi;
                }
            }
        }

        public IEnumerable<ButtonInfo> GetUsualButtons() {
            foreach (var btn in GetButtons().Where(btn => btn.ButtonType == EButtonType.Usual)) {
                yield return btn;
            }
        }

        public static implicit operator ButtonLayout(ButtonInfo[] btns) {
            var buttons = new List<List<ButtonInfo>> {
                btns.ToList()
            };

            return new ButtonLayout(buttons);
        }

        public static implicit operator ButtonLayout(ButtonInfo[,] btns) {
            return new ButtonLayout(btns.ToList());
        }

        public IEnumerator<List<ButtonInfo>> GetEnumerator() {
            return Buttons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Buttons.GetEnumerator();
        }
    }
}