using System;

namespace BusinessLayer.Model.API
{
	public class TicketUpdateStatusReq
	{
		public Guid AccountID { get; set; }

		public string Note { get; set; }

		public string AccountCode { get; set; }

		public string StatusOld { get; set; }

		public string StatusNew { get; set; }

		public Guid ComputerID { get; set; }

		public string ComputerUsingTicketID { get; set; }

		public string UpdateBy { get; set; }

		public string ServiceID { get; set; }

		public decimal TotalMoney { get; set; }

		public string SiteCode { get; set; }

		public string ComputerUsingTicketName { get; set; }
	}
}
