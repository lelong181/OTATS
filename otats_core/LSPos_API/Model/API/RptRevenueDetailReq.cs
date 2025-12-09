using System;

namespace BusinessLayer.Model.API
{
	public class RptRevenueDetailReq
	{
		public DateTime DateFrom { get; set; }

		public DateTime DateTo { get; set; }

		public string SiteCode { get; set; }

		public string Channel { get; set; }
	}
}
