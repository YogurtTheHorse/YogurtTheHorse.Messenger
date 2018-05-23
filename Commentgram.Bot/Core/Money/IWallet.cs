using System;
using System.Collections.Generic;
using System.Text;

namespace Commentgram.Bot.Core.Money {
	public interface IWallet {
		decimal Amount { get; }
	}
}
