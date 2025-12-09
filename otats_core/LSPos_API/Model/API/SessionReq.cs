using System;

namespace BusinessLayer.Model.API
{
	public class SessionReq
	{
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string Cashier { get; set; }

		public string SessionStatus { get; set; }

		public string SiteCode { get; set; }
	}
}
