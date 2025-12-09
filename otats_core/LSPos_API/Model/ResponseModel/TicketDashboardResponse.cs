using System;
using System.Collections.Generic;
using BusinessLayer.Model.API;

namespace Model.ResponseModel{

public class TicketDashboardResponse
{
	public IEnumerable<DsbTicketSellRes> TicketSell { get; set; }

	public IEnumerable<DsbTicketStatusRes> TicketStatus { get; set; }

	public IEnumerable<DsbTicketUsingACMRes> TicketUsingACM { get; set; }

	public DateTime DateView { get; set; }
}
}