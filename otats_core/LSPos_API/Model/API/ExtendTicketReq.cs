using System;

namespace BusinessLayer.Model.API
{
	public class ExtendTicketReq
	{
		public string AccountID { get; set; }

		public string AccountCode { get; set; }

		public DateTime ExtendUsingDate { get; set; }

		public string Username { get; set; }
	}
}
