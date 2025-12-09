using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class UgradeTicketReq
	{
		public DateTime CheckInDate { get; set; }

		public Guid InvoiceId { get; set; }

		public string SessionID { get; set; }

		public string Username { get; set; }

		public string ComputerID { get; set; }

		public string ZoneID { get; set; }

		public string SiteID { get; set; }

		public string SiteCode { get; set; }

		public string Note { get; set; }

		public string ProfileID { get; set; }

		public List<UgradeTicketDetailReq> UpgradeDetails { get; set; }

		public List<PaymentTypeReq> PaymentTypes { get; set; }

		public List<PaymentTypeReq> PaymentTypesRepay { get; set; }

		public UgradeTicketReq()
		{
			UpgradeDetails = new List<UgradeTicketDetailReq>();
			PaymentTypes = new List<PaymentTypeReq>();
		}
	}
}
