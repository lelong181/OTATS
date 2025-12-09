using System.Collections.Generic;
using BusinessLayer.Model.API;

namespace BusinessLayer.Model.Sell
{
	public class SaveBookingModel
	{
		public string InvoiceCode { get; set; }

		public string BookingCode { get; set; }

		public bool Success { get; set; }

		public bool IsPaymentFull { get; set; }

		public string InvoiceCodeRepay { get; set; }

		public List<BookingAccountResponse> Accounts { get; set; }
	}
}
