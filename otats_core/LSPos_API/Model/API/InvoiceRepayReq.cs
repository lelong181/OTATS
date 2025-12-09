using System;

namespace BusinessLayer.Model.API
{
	public class InvoiceRepayReq
	{
		public DateTime InvoiceDate { get; set; }

		public string InvoiceCode { get; set; }

		public string SiteCode { get; set; }
	}
}
