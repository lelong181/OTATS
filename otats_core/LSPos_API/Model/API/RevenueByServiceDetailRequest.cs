using System;

namespace BusinessLayer.Model.API
{
	public class RevenueByServiceDetailRequest
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string ServiceGroupCode { get; set; }

		public string ServiceSubGroupCode { get; set; }

		public string SiteID { get; set; }
	}
}
