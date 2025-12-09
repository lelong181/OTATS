using System;

namespace BusinessLayer.Model.API
{
	public class AccountTicketReq
	{
		public string InvoiceCode { get; set; }

		public string AccountCode { get; set; }

		public string Keyword { get; set; }

		public DateTime? FromDate { get; set; }

		public DateTime? ToDate { get; set; }
	}
}
