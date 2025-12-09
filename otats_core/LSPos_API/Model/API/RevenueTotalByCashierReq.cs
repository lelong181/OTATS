using System;

namespace BusinessLayer.Model.API
{
	public class RevenueTotalByCashierReq
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string CashierStr { get; set; }

		public string ServiceRateIDStr { get; set; }

		public string LangCode { get; set; }

		public string SiteCode { get; set; }
	}
}
