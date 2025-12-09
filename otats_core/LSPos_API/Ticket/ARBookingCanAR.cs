using System;

namespace Ticket
{
	public class ARBookingCanAR
	{
		public Guid ID { get; set; }

		public string BookingCode { get; set; }

		public string BookingStatus { get; set; }

		public Guid ProfileID { get; set; }

		public string ProfileName { get; set; }

		public decimal Amount { get; set; }

		public Guid PaymentTypeID { get; set; }
	}
}
