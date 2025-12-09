using System;

namespace BusinessLayer.Model.Reports
{
	public class RptCouponConfig
	{
		public string StatusText { get; set; }

		public string Code { get; set; }

		public string TypesText { get; set; }

		public string ServiceRateName { get; set; }

		public string ServiceDiscountName { get; set; }

		public string InactiveText { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public DateTime? BeginDate { get; set; }

		public DateTime? EndDate { get; set; }
	}
}
