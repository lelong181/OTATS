using System;

namespace BusinessLayer.Model.API
{
	public class RptCouponDateTimeRequest
	{
		public string SiteCode { get; set; }

		public string BookingCode { get; set; }

		public string Code { get; set; }

		public DateTime? FromDate { get; set; }

		public DateTime? ToDate { get; set; }
	}
}
