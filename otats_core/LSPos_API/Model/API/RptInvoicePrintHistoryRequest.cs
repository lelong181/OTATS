using System;

namespace BusinessLayer.Model.API
{
	public class RptInvoicePrintHistoryRequest
	{
		public DateTime TransactionDate { get; set; }

		public string Type { get; set; }
	}
}
