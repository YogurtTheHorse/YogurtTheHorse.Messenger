using System;
using Telegram.Bot.Types;

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
                originalUsr.Id.ToString() == usr.ID &&
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
    }
}
