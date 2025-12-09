using System;

namespace BusinessLayer.Model.API
{
	public class RptInvoiceByPaymentReq
	{
		public string Lang { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string Site { get; set; }

		public string Cashier { get; set; }
	}
}
