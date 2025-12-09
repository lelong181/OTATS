using System;

namespace BusinessLayer.Model.API
{
	public class DetailsServiceRequest
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string ServiceGroupID { get; set; }

		public string ServiceID { get; set; }

		public string Status { get; set; }

		public string SiteCode { get; set; }
	}
}
