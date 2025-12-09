using System;
using System.Collections.Generic;

namespace BusinessLayer.Model
{
	public class Promotion
	{
		public Guid ID { get; set; }

		public bool IsPercent { get; set; }

		public decimal DiscountAmount { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public TimeSpan? StartTime { get; set; }

		public TimeSpan? EndTime { get; set; }

		public string DayOfWeek { get; set; }

		public string PromoType { get; set; }

		public string SessionType { get; set; }

		public int FreeForMinOrder { get; set; }

		public int? MinOrder { get; set; }

		public bool? DiscountOrSurcharge { get; set; }

		public int? Day { get; set; }

		public bool Inactive { get; set; }

		public bool IsDelete { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }

		public virtual ICollection<PromotionLink> PromotionLinks { get; set; }

		public virtual ICollection<PromotionI18n> I18ns { get; set; }

		public Promotion()
		{
			PromotionLinks = new List<PromotionLink>();
			I18ns = new List<PromotionI18n>();
		}
	}
}
