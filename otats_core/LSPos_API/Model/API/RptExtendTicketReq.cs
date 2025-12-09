using System;

namespace BusinessLayer.Model.API
{
	public class RptExtendTicketReq
	{
		public DateTime StartActivityDate { get; set; }

		public DateTime EndActivityDate { get; set; }

		public string LangCode { get; set; }

		public string SiteCode { get; set; }
	}
}
