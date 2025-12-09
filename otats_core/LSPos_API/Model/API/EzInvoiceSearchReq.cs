using System;

namespace BusinessLayer.Model.API
{
	public class EzInvoiceSearchReq
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string CustomerName { get; set; }

		public string EzInvoiceCode { get; set; }

		public string CustomerTax { get; set; }
	}
}
