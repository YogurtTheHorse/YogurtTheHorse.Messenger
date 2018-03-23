using System.Threading.Tasks;
using YogurtTheHorse.Messenger.MenuControl;

namespace YogurtTheHorse.Messenger.Database {
	public interface IDatabaseDriver {
		void Connect();
		void Connect(string url);

		void RegisterUserMenuClass<TUserMenu>() where TUserMenu : IUserMenu;
		void RegisterUserDataClass<TUserData>() where TUserData : UserData;

		Task<User> GetUserAsync(string id);
		Task<bool> SaveUserAsync(User usr);

		User GetUser(string id);
		bool SaveUser(User usr);

		Task<UserData> GetUserDataAsync(string id);
		Task SaveUserDataAsync(UserData userData);

		UserData GetUserData(string id);
		void SaveUserData(UserData userData);

		Task<ImageInfo> TryGetImageAsync(string imageId);
	}
}
