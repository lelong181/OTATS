using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class BookingCreateRes
	{
		public bool IsPaymentFull { get; set; }

		public string BookingCode { get; set; }

		public string InvoiceCode { get; set; }

		public DepositRes Deposit { get; set; }

		public string InvoiceCodeRepay { get; set; }

		public List<BookingAccountResponse> BookingAccounts { get; set; }
	}
}
