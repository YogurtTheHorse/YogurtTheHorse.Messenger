using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;
using YogurtTheHorse.Messenger.MenuControl.Buttons;

namespace YogurtTheHorse.Messenger.Telegram {
	internal static class Extensions {
		public static MessageType ToYogurtType(this global::Telegram.Bot.Types.Enums.MessageType msgType) {
			switch (msgType) {
				case global::Telegram.Bot.Types.Enums.MessageType.TextMessage:
					return MessageType.Text;

				default:
					return MessageType.Other;
			}
		}

		public static bool UserEquals(this global::Telegram.Bot.Types.User originalUsr, User usr) {
			return
				usr.PlatformName == TelegramMessenger.PLATFORM_NAME &&
				originalUsr.Id.ToString() == usr.UserID &&
				originalUsr.FirstName == usr.FirstName &&
				originalUsr.LastName == usr.LastName &&
				originalUsr.Username == usr.Username &&
				originalUsr.LanguageCode == usr.LanguageCode;
		}

		public static bool UpdateUser(this global::Telegram.Bot.Types.User originalUsr, User usr) {
			if (UserEquals(originalUsr, usr)) {
				return false;
			}

			usr.FirstName = originalUsr.FirstName;
			usr.LastName = originalUsr.LastName;
			usr.LanguageCode = originalUsr.LanguageCode;
			usr.Username = originalUsr.Username;

			return true;
		}

		public static FileToSend ImageToFileToSend(ImageInfo imageInfo) {
			throw new NotImplementedException();
		}

		public static KeyboardButton ToTelegramButton(this ButtonInfo button) {
			switch (button.ButtonType) {
				case EButtonType.Inline:
					return InlineKeyboardButton.WithCallbackData(button.Text, button.Data);

				case EButtonType.Usual:
					return new KeyboardButton(button.Text);

				default:
					throw new NotSupportedException();
			}
		}

		public static IReplyMarkup ToReplyMarkup(this ButtonLayout layout) {
			ButtonInfo[][] buttons = layout.GetButtons();
			KeyboardButton[][] keyboardButtons = new KeyboardButton[buttons.Length][];

			for (int i = 0; i < buttons.Length; i++) {
				keyboardButtons[i] = new KeyboardButton[buttons[i].Length];

				for (int j = 0; j < buttons[i].Length; j++) {
					keyboardButtons[i][j] = buttons[i][j].ToTelegramButton();
				}
			}

			return new ReplyKeyboardMarkup(keyboardButtons, layout.OneTimeKeyboard, layout.ResizeKeyboard);
		}
	}
}
