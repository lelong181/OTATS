using System;

namespace BusinessLayer.Model.Sell
{
	public class PromotionResponse
	{
		public Guid? PromotionID { get; set; }

		public Guid PromotionLinkID { get; set; }

		public string Title { get; set; }

		public decimal DiscountAmount { get; set; }

		public bool IsDefault { get; set; }

		public bool IsPercent { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public DateTime? StartTime { get; set; }

		public DateTime? EndTime { get; set; }

		public string PromoType { get; set; }

		public string SessionType { get; set; }

		public int FreeForMinOrder { get; set; }

		public int? MinOrder { get; set; }

		public bool? DiscountOrSurcharge { get; set; }

		public string Day { get; set; }

		public Guid? ServiceRateID { get; set; }

		public Guid? ServiceID { get; set; }

		public string PromotionName { get; set; }

		public decimal ActualDiscountAmount { get; set; }
	}
}
