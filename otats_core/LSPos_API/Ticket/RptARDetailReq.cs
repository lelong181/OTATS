using System;

namespace Ticket
{
	public class RptARDetailReq
	{
		public string AccountNo { get; set; }

		public DateTime? Fromdate { get; set; }

		public DateTime? Todate { get; set; }

		public string ProfileInfo { get; set; }

		public int Type { get; set; }
	}
}
