using System;
using System.Collections.Generic;

namespace BusinessLayer.Model
{
	public class ServiceRateRepaySelected
	{
		public Guid? InvoiceID { get; set; }

		public string InvoiceCode { get; set; }

		public Guid ServiceRateID { get; set; }

		public decimal? Price { get; set; }

		public int Quantity { get; set; }

		public decimal Amount { get; set; }

		public string ServiceRateName { get; set; }

		public string PromotionName { get; set; }

		public Guid? PromotionID { get; set; }

		public Guid? CouponID { get; set; }

		public List<AccountRepaySelected> ListAccount { get; set; }

		public ServiceRateRepaySelected()
		{
			ListAccount = new List<AccountRepaySelected>();
		}
	}
}
