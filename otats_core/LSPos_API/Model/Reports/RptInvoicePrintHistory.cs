using System;

namespace BusinessLayer.Model.Reports
{
	public class RptInvoicePrintHistory
	{
		public decimal TotalMoney { get; set; }

		public string FullName { get; set; }

		public DateTime PrintTime { get; set; }

		public string InvoiceCode { get; set; }
	}
}
