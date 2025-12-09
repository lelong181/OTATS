using System;

namespace BusinessLayer.Model.API
{
	public class RptBookingUpgradeReq
	{
		public string LangCode { get; set; }

		public string SiteCode { get; set; }

		public string Cashier { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }
	}
}
