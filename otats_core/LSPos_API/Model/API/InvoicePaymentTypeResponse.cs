using System;

namespace BusinessLayer.Model.API
{
	public class InvoicePaymentTypeResponse
	{
		public Guid PaymentTypeID { get; set; }

		public decimal Amount { get; set; }

		public decimal InputAmount { get; set; }

		public string Title { get; set; }

		public bool? IsDeposit { get; set; }
	}
}
