using System;

namespace BusinessLayer.Model
{
	public class CreateWalletRes
	{
		public string WalletCode { get; set; }

		public Guid AccountID { get; set; }

		public Guid AccountDetailID { get; set; }

		public string Eror { get; set; }
	}
}
