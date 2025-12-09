using System;
using System.Collections.Generic;

namespace BusinessLayer.Model
{
	public class BookingDetailSeatReq
	{
		public string Id { get; set; }

		public int Quantity { get; set; }

		public string Checkin { get; set; }

		public Guid? ShiftID { get; set; }

		public Guid? _ZoneID { get; set; }

		public List<LockSeat> listSeatLock { get; set; }

		public BookingDetailSeatReq()
		{
			listSeatLock = new List<LockSeat>();
		}

		public Guid GetRateId()
		{
			return Guid.Parse(Id);
		}
	}
}
