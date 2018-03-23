using System.Threading.Tasks;
using YogurtTheHorse.Messenger.Database;
using YogurtTheHorse.Messenger.MenuControl;

namespace YogurtTheHorse.Messenger {
    public delegate void MessageEventHandler(Message msg);
    
	public interface IMessenger {
		string PlatformName { get; }
		
		IDatabaseDriver Database { get; }

		Task<User> GetUserAsync(string id);
		Task<bool> SaveUserAsync(User usr);

		Task SendMessageAsync(Message message);
		void SendMessage(Message message);

		void Launch();

		event MessageEventHandler OnIncomingMessage;
	}
}
