using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class BookingListPosResult
	{
		public List<ProcBookingListPos> Bookings { get; set; }

		public int Total { get; set; }

		public BookingListPosResult()
		{
			Bookings = new List<ProcBookingListPos>();
		}
	}
}
