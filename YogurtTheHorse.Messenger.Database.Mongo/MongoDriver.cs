using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;

using System.Threading.Tasks;
using YogurtTheHorse.Messenger.MenuControl;

namespace YogurtTheHorse.Messenger.Database.Mongo {
    public class MongoDriver : IDatabaseDriver {
        private IMongoDatabase _database;
        private IMongoCollection<User> _usersCollection;
        private IMongoCollection<IUserData> _usersDataCollection;

        public string DatabaseName { get; protected set; }

        public MongoDriver(string databaseName) {
            DatabaseName = databaseName;

            BsonClassMap.RegisterClassMap<User>(cm => {
                cm.AutoMap();
            });
        }

        public void Connect() {
            Connect("mongodb://localhost:27017");
        }

        public void Connect(string url) {
            MongoClient mongo = new MongoClient(url);

            _database = mongo.GetDatabase(DatabaseName);
            _usersCollection = _database.GetCollection<User>("users");
        }

        public async Task<User> GetUserAsync(string id) {
            return await _usersCollection.Find(u => u.ID == id).SingleAsync();
        }

        public async Task<bool> SaveUserAsync(User usr) {
            return (await _usersCollection.ReplaceOneAsync(u => u.ID == usr.ID, usr, new UpdateOptions { IsUpsert = true })).MatchedCount > 0;
        }

        public Task<ImageInfo> TryGetImageAsync(string imageId) {
            throw new NotImplementedException();
        }

        public async Task<IUserData> GetUserDataAsync(string id) {
            return await _usersDataCollection.Find(ud => ud.ID == id).SingleAsync();
        }

        public async Task SaveUserDataAsync(IUserData userData) {
            //if (!BsonClassMap.IsClassMapRegistered(userData.Menu.GetType())) {
            //    typeof(MongoDriver).GetMethod("RegisterUserMenuClass").MakeGenericMethod(userData.Menu.GetType()).Invoke(this, null);
            //}

            await _usersDataCollection.ReplaceOneAsync(u => u.ID == userData.ID, userData, new UpdateOptions { IsUpsert = true });
        }

        public void RegisterUserDataType<TUserData>() where TUserData : IUserData {
            if (_usersCollection != null) {
                throw new InvalidOperationException("User data type is already registered");
            }

            BsonClassMap.RegisterClassMap<TUserData>(cm => cm.AutoMap());
            _usersDataCollection = (IMongoCollection<IUserData>)_database.GetCollection<TUserData>("users_data");
        }

        public void RegisterUserMenuClass<TUserMenu>() where TUserMenu : IUserMenu {
            BsonClassMap.RegisterClassMap<TUserMenu>(cm => cm.AutoMap());
        }

        public User GetUser(string id) {
            var task = GetUserAsync(id);
            task.RunSynchronously();
            return task.Result;
        }

        public bool SaveUser(User usr) {
            var task = SaveUserAsync(usr);
            task.RunSynchronously();
            return task.Result;
        }

        public IUserData GetUserData(string id) {
            var task = GetUserDataAsync(id);
            task.RunSynchronously();
            return task.Result;
        }

        public void SaveUserData(IUserData userData) {
            SaveUserDataAsync(userData).RunSynchronously();
        }
	}
}
