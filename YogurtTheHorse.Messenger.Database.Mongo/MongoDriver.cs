using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using YogurtTheHorse.Messenger.MenuControl;

namespace YogurtTheHorse.Messenger.Database.Mongo {
	public class MongoDriver<TUserData> : IDatabaseDriver<TUserData> where TUserData : IUserData {
		private IMongoDatabase _database;
		private IMongoCollection<User> _usersCollection;
		private IMongoCollection<TUserData> _usersDataCollection;

		public string DatabaseName { get; protected set; }

		public MongoDriver(string databaseName) {
			DatabaseName = databaseName;
			
			BsonClassMap.RegisterClassMap<User>(cm => {
				cm.AutoMap();
				cm.MapProperty(c => c.UserID);
				cm.UnmapMember(c => c.Messenger);
			});

			BsonClassMap.RegisterClassMap<TUserData>(cm => {
				cm.AutoMap();
				cm.MapProperty(c => c.UserID);
			});
		}

		public void Connect() {
			Connect("mongodb://localhost:27017");
		}

		public void Connect(string url) {
			MongoClient mongo = new MongoClient(url);

			_database = mongo.GetDatabase(DatabaseName);
			_usersCollection = _database.GetCollection<User>("users");

			_usersDataCollection = _database.GetCollection<TUserData>("users_data");
		}

		public async Task<User> GetUserAsync(string id) {
			FilterDefinition<User> filter = $"{{ UserId: {id} }}";
			return (await _usersCollection.Find(filter).Limit(1).ToListAsync()).FirstOrDefault();
		}

		public async Task<bool> SaveUserAsync(User usr) {
			FilterDefinition<User> filter = $"{{ UserId: {usr.UserID} }}";
			var result = await _usersCollection.ReplaceOneAsync(filter, usr, new UpdateOptions { IsUpsert = true });

			return result.MatchedCount > 0;
		}

		public Task<ImageInfo> TryGetImageAsync(string imageId) {
			throw new NotImplementedException();
		}

		public async Task<TUserData> GetUserDataAsync(string id) {
			FilterDefinition<TUserData> filter = $"{{ UserID: {id}}}";
			var results = await _usersDataCollection.Find(filter).Limit(1).ToListAsync();

			return results.FirstOrDefault();
		}

		public async Task SaveUserDataAsync(TUserData userData) {
			FilterDefinition<TUserData> filter = $"{{ UserID: {userData.UserID}}}";
			await _usersDataCollection.ReplaceOneAsync(filter, userData, new UpdateOptions { IsUpsert = true });
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

		public TUserData GetUserData(string id) {
			return GetUserDataAsync(id).GetAwaiter().GetResult();
		}

		public void SaveUserData(TUserData userData) {
			SaveUserDataAsync(userData).RunSynchronously();
		}
	}
}
