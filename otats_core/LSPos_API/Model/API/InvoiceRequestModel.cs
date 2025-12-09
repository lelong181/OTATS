using System;

namespace BusinessLayer.Model.API
{
	public class InvoiceRequestModel
	{
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string InvoiceCode { get; set; }

		public string AccountCode { get; set; }

		public string SiteCode { get; set; }

		public string Type { get; set; }

		public string CashierID { get; set; }

		public int QueryTop { get; set; }
	}
}
