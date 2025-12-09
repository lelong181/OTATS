using System;

namespace BusinessLayer.Model.Reports
{
	public class RptTicketPrintHistory
	{
		public string ServiceName { get; set; }

		public decimal TotalMoney { get; set; }

		public string AccountCode { get; set; }

		public string FullName { get; set; }

		public DateTime PrintTime { get; set; }

		public string InvoiceCode { get; set; }
	}
}
