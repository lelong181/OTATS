using System;

namespace BusinessLayer.Model.API
{
	public class RptWalletDepositRes
	{
		public string Status { get; set; }

		public string AccountCode { get; set; }

		public DateTime? TransactionDate { get; set; }

		public string Description { get; set; }

		public decimal Amount { get; set; }

		public string ServiceName { get; set; }

		public string SessionNo { get; set; }

		public string Username { get; set; }

		public DateTime? PaymentDate { get; set; }
	}
}
