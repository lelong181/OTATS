using System;

namespace BusinessLayer.Model.Reports
{
	public class RptCoupon
	{
		public string StatusText { get; set; }

		public string Code { get; set; }

		public int QuantityMax { get; set; }

		public int QuantityUsed { get; set; }

		public string TypeText { get; set; }

		public decimal TypeValues { get; set; }

		public string DiscountTypeText { get; set; }

		public decimal DiscountTypeValues { get; set; }

		public string DayOfWeek { get; set; }

		public decimal DiscountTypeValueMax { get; set; }

		public string OptionText { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public DateTime? BeginDate { get; set; }

		public DateTime? EndDate { get; set; }
	}
}
