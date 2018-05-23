using System;
using System.Collections.Generic;
using System.Text;

namespace Commentgram.Bot.Core {
	public class InstagramLink {
		public string Text { get; set; }
		public uint CommentsCount { get; set; }
		public string AuthorUID { get; set; }

		public InstagramLink() {
			CommentsCount = 0;
		}
	}
}
