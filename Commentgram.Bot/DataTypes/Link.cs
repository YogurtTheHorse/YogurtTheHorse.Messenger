using System.Linq;
using YogurtTheHorse.Messenger.Database;

namespace Commentgram.Bot.DataTypes {
	public class Link {
		public string AuthorID { get; set; }
		public string Path { get; set; }
		public uint CommentsCount { get; set; }
		public bool Approved { get; set; }

		public Link() {
			AuthorID = null;
			Path = null;
			CommentsCount = 0;
			Approved = false;
		}

		public Link(string author, string path) : this() {
			AuthorID = author;
			Path = path;
		}

		public Link(string author, string path, uint commentsCount) : this(author, path) {
			CommentsCount = commentsCount;
		}

		public uint GetCommentsPublished(IDatabaseDriver database) {
			return (uint)database.GetQueryable<Comment>("comments").Where(c => c.LinkToPost == Path).Count();
		}
	}
}
