using System;

namespace BusinessLayer.Model.API
{
	public class RevenueByMonthRequest
	{
		public DateTime Date { get; set; }

		public string ServiceGroupCode { get; set; }

		public string ServiceSubGroupCode { get; set; }

		public string SiteCode { get; set; }
	}
}
