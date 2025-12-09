using System.Collections.Generic;

namespace BusinessLayer.Model.Wallet
{
	public class WalletInvInforReq
	{
		public string WalletCode { get; set; }

		public string DecCode { get; set; }

		public List<int> InvList { get; set; }
	}
}
