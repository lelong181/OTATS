using System;

namespace BusinessLayer.Model.API
{
	public class BookingDetaiInfolReq
	{
		public Guid BookingID { get; set; }

		public string SiteCode { get; set; }

		public string LangCode { get; set; }
	}
}
