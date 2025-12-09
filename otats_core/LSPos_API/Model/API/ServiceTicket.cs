using System;

namespace BusinessLayer.Model.API
{
	public class ServiceTicket
	{
		public Guid? ServiceRateID { get; set; }

		public Guid? ServiceID { get; set; }

		public string ServiceName { get; set; }

		public string CardType { get; set; }

		public bool? PrintTicketBy { get; set; }
	}
}
