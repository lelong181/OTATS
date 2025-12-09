using System;

namespace BusinessLayer.Model.API
{
	public class RptDetailPaymentTypeRes
	{
		public DateTime? TransactionDate { get; set; }

		public string InvoiceCode { get; set; }

		public string BookingCode { get; set; }

		public string IdentityCard { get; set; }

		public string ProfileName { get; set; }

		public string ProfileCode { get; set; }

		public string Address { get; set; }

		public string FullName { get; set; }

		public string Cashier { get; set; }

		public string Username { get; set; }

		public string ComputerName { get; set; }

		public decimal Amount { get; set; }

		public string PaymentType { get; set; }

		public string Description { get; set; }

		public string Channel { get; set; }

		public string Source { get; set; }

		public int SessionNo { get; set; }

		public string BookingStatusStr { get; set; }

		public DateTime? CheckinDate { get; set; }

		public DateTime? InvoiceDate { get; set; }
	}
}
