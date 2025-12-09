using System;

namespace BusinessLayer.Model.API
{
	public class DetailUseServiceByAcmRequest
	{
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string SiteCode { get; set; }

		public string ServiceId { get; set; }

		public string ServiceSubGroupId { get; set; }
	}
}
