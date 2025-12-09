using System;

namespace BusinessLayer.Model.API
{
	public class ActiveTicketBookingB2BReq
	{
		public Guid BookingID { get; set; }

		public string CreatedBy { get; set; }
	}
}
