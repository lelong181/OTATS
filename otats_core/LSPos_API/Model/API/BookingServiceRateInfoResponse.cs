using System;

namespace BusinessLayer.Model.API
{
	public class BookingServiceRateInfoResponse
	{
		public Guid? ServiceRateID { get; set; }

		public string ServiceRateName { get; set; }

		public string PromotionName { get; set; }

		public int? Quantity { get; set; }

		public decimal? Price { get; set; }

		public decimal? Amount { get; set; }
	}
}
