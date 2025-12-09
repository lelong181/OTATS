using System;

namespace BaseBusiness
{
	public class PromotionModel
	{
		public Guid PromotionID { get; set; }

		public string Title { get; set; }

		public decimal DiscountAmount { get; set; }

		public bool IsDefault { get; set; }

		public bool IsPercent { get; set; }

		public DateTime? StartTime { get; set; }

		public DateTime? EndTime { get; set; }

		public string PromoType { get; set; }

		public string SessionType { get; set; }

		public int FreeForMinOrder { get; set; }

		public int? MinOrder { get; set; }

		public bool? DiscountOrSurcharge { get; set; }

		public string ServiceRateID { get; set; }
	}
}
