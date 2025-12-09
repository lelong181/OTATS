using System;

namespace BusinessLayer.Model
{
	public class ServiceRateAdjustRes
	{
		public Guid? InvoiceID { get; set; }

		public string InvoiceCode { get; set; }

		public Guid ServiceID { get; set; }

		public Guid ServiceRateID { get; set; }

		public decimal? Price { get; set; }

		public decimal? PriceAdjust { get; set; }

		public int Quantity { get; set; }

		public decimal? Amount { get; set; }

		public string ServiceRateName { get; set; }

		public string ServiceName { get; set; }

		public string AdjustServiceCode { get; set; }

		public string AdjustServiceName { get; set; }

		public string PromotionName { get; set; }

		public Guid? PromotionID { get; set; }
	}
}
