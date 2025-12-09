using System;

namespace BusinessLayer.Model.Wallet
{
	public class CreatedWalletReq
	{
		public string WalletCode { get; set; }

		public Guid? AccountID { get; set; }

		public string MemberID { get; set; }

		public decimal CreditLimit { get; set; }

		public string Description { get; set; }

		public string SiteCode { get; set; }

		public string CreatedBy { get; set; }

		public string ServiceWalletID { get; set; }
	}
}
