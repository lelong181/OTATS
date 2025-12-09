using System;

namespace BusinessLayer.Model.API
{
	public class RevenueSummaryBookingB2B
	{
		public string ProfileCode { get; set; }

		public string ProfileName { get; set; }

		public string TaxNo { get; set; }

		public string BookingStatus { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string SiteId { get; set; }
	}
}
