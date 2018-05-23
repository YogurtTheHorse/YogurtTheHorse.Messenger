using System;
using System.Collections.Generic;

namespace YogurtTheHorse.Messenger.Localizations {
	public static class LocaleManager {
		public static string AnyLocale => "Any";

		private static Dictionary<string, ILocale> _locales;

		static LocaleManager() {
			_locales = new Dictionary<string, ILocale>();
		}

		public static string Format(string v, string locale, params object[] args) {
			return String.Format(GetString(v, locale), args);
		}

		public static string GetString(string v, string localeName) {
			return GetString(v, localeName, $"{{{v}}} not forund for {localeName} locale (may be it's missing)");
		}

		public static IEnumerable<string> GetLocalesNames() {
			return _locales.Keys;
		}

		public static string GetString(string v, string localeName, string defaultValue) {
			if (localeName == AnyLocale) {
				foreach (ILocale locale in _locales.Values) {
					if (locale.ContainsString(v)) {
						return locale.GetString(v, defaultValue);
					}
				}

			}

			if (GetLocale(localeName, out ILocale locale_)) {
				locale_.GetString(v, defaultValue);
			}

			return defaultValue;
		}

		private static bool GetLocale(string localeName, out ILocale locale) {
			return _locales.TryGetValue(localeName, out locale);
		}
	}
}
