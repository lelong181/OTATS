using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class UgradeServiceRateDetailReq
	{
		public Guid OriginServiceRateID { get; set; }

		public Guid UpgradeServiceRateID { get; set; }

		public Guid? PromotionID { get; set; }

		public decimal OldPrice { get; set; }

		public decimal NewPrice { get; set; }

		public List<UgradeAccountDetailReq> ListAccount { get; set; }

		public UgradeServiceRateDetailReq()
		{
			ListAccount = new List<UgradeAccountDetailReq>();
		}
	}
}
