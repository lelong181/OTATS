using System;

namespace BusinessLayer.Model.API
{
	public class UgradeTicketDetailReq
	{
		public string AccountCode { get; set; }

		public Guid AccountID { get; set; }

		public Guid ServicePackageID { get; set; }

		public Guid UpgradeServiceRateID { get; set; }
	}
}
