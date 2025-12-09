using System;

namespace BusinessLayer.Model.API
{
	public class RevenueByServicePostingJournalRequest
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string ServiceGroupCode { get; set; }

		public string ServiceSubGroupCode { get; set; }

		public string ServiceCode { get; set; }

		public int Type { get; set; }

		public string SiteID { get; set; }
	}
}
