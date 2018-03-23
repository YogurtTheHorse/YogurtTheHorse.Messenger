using System.Threading.Tasks;
using YogurtTheHorse.Messenger.MenuControl;

namespace YogurtTheHorse.Messenger.Database {
	public interface IDatabaseDriver {
		void Connect();
		void Connect(string url);

		void RegisterUserMenuClass<TUserMenu>() where TUserMenu : IUserMenu;

		Task<User> GetUserAsync(string id);
		Task<bool> SaveUserAsync(User usr);

		User GetUser(string id);
		bool SaveUser(User usr);

		Task<ImageInfo> TryGetImageAsync(string imageId);
	}

	public interface IDatabaseDriver<TUserData> : IDatabaseDriver where TUserData : IUserData {
		Task<TUserData> GetUserDataAsync(string id);
		Task SaveUserDataAsync(TUserData userData);

		TUserData GetUserData(string id);
		void SaveUserData(TUserData userData);
	}
}
