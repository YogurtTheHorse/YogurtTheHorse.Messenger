using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using YogurtTheHorse.Messenger.MenuControl;
using YogurtTheHorse.Messenger.MenuControl.Menus;

namespace YogurtTheHorse.Messenger.Database.Mongo {
	public class MongoDriver : IDatabaseDriver {
		private IMongoDatabase _database;
		private IMongoCollection<User> _usersCollection;
		private IMongoCollection<UserData> _usersDataCollection;

		public string DatabaseName { get; protected set; }

		public MongoDriver(string databaseName) {
			DatabaseName = databaseName;
			
			BsonClassMap.RegisterClassMap<User>(cm => {
				cm.AutoMap();
				cm.SetIdMember(cm.GetMemberMap(c => c.UserID));
				cm.UnmapMember(c => c.Messenger);
			});

			BsonClassMap.RegisterClassMap<UserData>(cm => {
				cm.AutoMap();
				cm.SetIgnoreExtraElements(true);
			});
		}

		public void RegisterUserDataClass<TUserData>() where TUserData : UserData {
			BsonClassMap.RegisterClassMap<TUserData>(cm => {
				cm.AutoMap();
				cm.SetIgnoreExtraElements(true);
			});
		}

		public void Connect() {
			Connect("mongodb://localhost:27017");
		}

		public void Connect(string url) {
			MongoClient mongo = new MongoClient(url);

			_database = mongo.GetDatabase(DatabaseName);
			_usersCollection = _database.GetCollection<User>("users");

			_usersDataCollection = _database.GetCollection<UserData>("users_data");
		}

		public async Task<User> GetUserAsync(string id) {
			return (await _usersCollection.Find(GetUserFilter(id)).Limit(1).ToListAsync()).FirstOrDefault();
		}

		public async Task<bool> SaveUserAsync(User usr) {
			var result = await _usersCollection.ReplaceOneAsync(GetUserFilter(usr.UserID), usr, new UpdateOptions { IsUpsert = true });

			return result.MatchedCount > 0;
		}

		public Task<ImageInfo> TryGetImageAsync(string imageId) {
			throw new NotImplementedException();
		}

		public async Task<UserData> GetUserDataAsync(string id) {
			var results = await _usersDataCollection.Find(GetUserDataFilter(id)).Limit(1).ToListAsync();

			return results.FirstOrDefault();
		}

		public async Task SaveUserDataAsync(UserData userData) {
			await _usersDataCollection.ReplaceOneAsync(GetUserDataFilter(userData.UserID), userData, new UpdateOptions { IsUpsert = true });
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

		public UserData GetUserData(string id) {
			return GetUserDataAsync(id).GetAwaiter().GetResult();
		}

		public void SaveUserData(UserData userData) {
			AsyncHelpers.RunSync(() => SaveUserDataAsync(userData));
		}

		private FilterDefinition<UserData> GetUserDataFilter(string id) {
			return Builders<UserData>.Filter.Eq(u => u.UserID, id);
		}

		private FilterDefinition<User> GetUserFilter(string id) {
			return Builders<User>.Filter.Eq(u => u.UserID, id);
		}
	}
}
