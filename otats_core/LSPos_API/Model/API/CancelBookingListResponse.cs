using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class CancelBookingListResponse
	{
		public List<BookingRes> Bookings { get; set; }

		public int TotalBookings { get; set; }

		public List<ServiceRes> ServiceReses { get; set; }
	}
}
