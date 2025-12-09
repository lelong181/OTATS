using System;

namespace BusinessLayer.Model.API
{
	public class InvoiceAdjustReq
	{
		public DateTime? InvoiceDate { get; set; }

		public string InvoiceCode { get; set; }

		public string SiteCode { get; set; }
	}
}
