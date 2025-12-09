using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class RepayServiceRateDetailReq
	{
		public Guid ServiceRateID { get; set; }

		public int Quantity { get; set; }

		public Guid? PromotionID { get; set; }

		public Guid? CouponID { get; set; }

		public List<RepayAccountDetailReq> Accounts { get; set; }
	}
}
