using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class TicketDashboardResponse
	{
		public IEnumerable<DsbTicketSellRes> TicketSell { get; set; }

		public IEnumerable<DsbTicketStatusRes> TicketStatus { get; set; }

		public IEnumerable<DsbTicketUsingACMRes> TicketUsingACM { get; set; }

		public DateTime DateView { get; set; }
	}
}
