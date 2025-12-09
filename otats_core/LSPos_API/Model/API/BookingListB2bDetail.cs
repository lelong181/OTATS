using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class BookingListB2bDetail
	{
		public List<BookingB2bDetail> Bookings { get; set; }

		public int Total { get; set; }

		public BookingListB2bDetail()
		{
			Bookings = new List<BookingB2bDetail>();
		}
	}
}
