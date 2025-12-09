using System;

namespace BusinessLayer.Model.API
{
	public class RptDetailPaymentTypeReq
	{
		public string LangCode { get; set; }

		public string SiteCode { get; set; }

		public string Channels { get; set; }

		public string Cashier { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }
	}
}
