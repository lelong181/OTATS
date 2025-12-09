using System;

namespace BusinessLayer.Model.API
{
	public class RptAcmCountByDateReq
	{
		public string LangCode { get; set; }

		public string SiteCode { get; set; }

		public DateTime Date { get; set; }
	}
}
