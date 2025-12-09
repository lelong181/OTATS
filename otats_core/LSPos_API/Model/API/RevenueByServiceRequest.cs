using System;

namespace BusinessLayer.Model.API
{
	public class RevenueByServiceRequest
	{
		public DateTime TransactionDate { get; set; }

		public string ServiceGroupCode { get; set; }

		public string ServiceSubGroupCode { get; set; }

		public int Type { get; set; }

		public string SiteCode { get; set; }
	}
}
