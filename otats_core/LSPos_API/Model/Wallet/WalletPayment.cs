using System;

namespace BusinessLayer.Model.Wallet
{
	public class WalletPayment
	{
		public Guid PaymentID { get; set; }

		public string PaymentName { get; set; }

		public decimal Amount { get; set; }

		public DateTime PaymentDate { get; set; }

		public string CreatedDate { get; set; }

		public int SessionNo { get; set; }

		public string Cashier { get; set; }

		public string ComputerName { get; set; }

		public string ZoneName { get; set; }
	}
}
