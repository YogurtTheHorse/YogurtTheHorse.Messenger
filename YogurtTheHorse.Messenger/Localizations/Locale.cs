using System.Collections.Generic;

namespace YogurtTheHorse.Messenger.Localizations {
	public class DictionaryLocale : Dictionary<string, string>, ILocale {
		public bool ContainsString(string name) {
			return ContainsKey(name);
		}

		public string GetString(string name) {
			return GetString(name, null);
		}
		public string GetString(string name, string defaultValue) {
			return TryGetValue(name, out string result) ? result : defaultValue;
		}
	}
}