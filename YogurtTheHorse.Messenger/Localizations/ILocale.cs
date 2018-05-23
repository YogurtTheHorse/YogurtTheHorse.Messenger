namespace YogurtTheHorse.Messenger.Localizations {
	public interface ILocale {
		string GetString(string name);
		string GetString(string name, string defaultValue);
		bool ContainsString(string name);
	}
}