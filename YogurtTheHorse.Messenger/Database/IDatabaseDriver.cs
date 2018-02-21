using System.Threading.Tasks;
using YogurtTheHorse.Messenger.MenuControl;

namespace YogurtTheHorse.Messenger.Database {
    public interface IDatabaseDriver {
        void Connect();
        void Connect(string url);

        void RegisterUserDataType<T>() where T : IUserData;
        void RegisterUserMenuClass<TUserMenu>() where TUserMenu : IUserMenu;

        Task<User> GetUserAsync(string id);
        Task<bool> SaveUserAsync(User usr);

        User GetUser(string id);
        bool SaveUser(User usr);

        Task<IUserData> GetUserDataAsync(string id);
        Task SaveUserDataAsync<TUserData>(TUserData userData) where TUserData : IUserData;

        IUserData GetUserData(string id);
        void SaveUserData<TUserData>(TUserData userData) where TUserData : IUserData;

        Task<ImageInfo> TryGetImageAsync(string imageId);
    }
}
