using System;

namespace BusinessLayer.Model.API
{
	public class BookingPaymentResponse
	{
		public Guid ID { get; set; }

		public decimal Amount { get; set; }

		public string CurrencyCode { get; set; }

		public DateTime TransactionDate { get; set; }

		public string ResponseMessage { get; set; }

		public string ResponseCode { get; set; }

		public Guid ServiceID { get; set; }

		public string ServiceName { get; set; }

		public bool? IsPaymentDeposit { get; set; }
	}
}
