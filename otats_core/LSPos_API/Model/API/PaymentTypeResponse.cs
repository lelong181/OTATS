using System;

namespace BusinessLayer.Model.API
{
	public class PaymentTypeResponse
	{
		public Guid InvoicePaymentTypeID { get; set; }

		public Guid PaymentTypeID { get; set; }

		public decimal Amount { get; set; }

		public string Title { get; set; }
	}
}
