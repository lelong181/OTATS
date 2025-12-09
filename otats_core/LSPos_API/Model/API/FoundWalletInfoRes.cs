using System;

namespace BusinessLayer.Model.API
{
	public class FoundWalletInfoRes
	{
		public string AccountCode { get; set; }

		public decimal? TotalMoney { get; set; }

		public decimal? CreditLimit { get; set; }

		public string Description { get; set; }

		public DateTime? TransactionDate { get; set; }

		public string Email { get; set; }

		public string IdOrPPNum { get; set; }

		public string Name { get; set; }

		public string PhoneNumber { get; set; }

		public string InvoiceCode { get; set; }

		public string BookingCode { get; set; }

		public string LockerName { get; set; }

		public string LockerZoneName { get; set; }

		public string LockerCode { get; set; }
	}
}
