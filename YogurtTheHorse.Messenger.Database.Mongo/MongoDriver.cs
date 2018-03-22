﻿using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using YogurtTheHorse.Messenger.MenuControl;

namespace YogurtTheHorse.Messenger.Database.Mongo {
	public class MongoDriver<TUserData> : IDatabaseDriver where TUserData : IUserData {
		private IMongoDatabase _database;
		private IMongoCollection<User> _usersCollection;
		private IMongoCollection<TUserData> _usersDataCollection;

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


			BsonClassMap.RegisterClassMap<TUserData>(cm => cm.AutoMap());

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

		public async Task<IUserData> GetUserDataAsync(string id) {
			FilterDefinition<TUserData> filter = $"{{ UserID: {id}}}";
			var results = await _usersDataCollection.Find(filter).Limit(1).ToListAsync();

			return results.FirstOrDefault();
		}

		public async Task SaveUserDataAsync(IUserData userData) {
			FilterDefinition<TUserData> filter = $"{{ ID: {userData.UserID}}}";
			await _usersDataCollection.ReplaceOneAsync(filter, (TUserData)userData, new UpdateOptions { IsUpsert = true });
		}

		public void RegisterUserDataType<T>() where T : IUserData {
			if (typeof(T) != typeof(TUserData)) {
				throw new InvalidOperationException("User data type is already registered");
			}
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
			return GetUserDataAsync(id).GetAwaiter().GetResult();
		}

		public void SaveUserData(IUserData userData) {
			SaveUserDataAsync(userData).RunSynchronously();
		}
	}
}
