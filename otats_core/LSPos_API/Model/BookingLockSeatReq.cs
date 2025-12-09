using System;
using System.Collections.Generic;

namespace BusinessLayer.Model
{
	public class BookingLockSeatReq
	{
		public string Checkin { get; set; }

		public string SiteId { get; set; }

		public string BookingCode { get; set; }

		public List<BookingDetailSeatReq> Details { get; set; }

		public Guid? ShiftID { get; set; }

		public string CreatedBy { get; set; }

		public BookingLockSeatReq()
		{
			Details = new List<BookingDetailSeatReq>();
		}
	}
}
