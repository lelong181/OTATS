using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class BookingDetailReq
	{
		public string Title { get; set; }

		public long Amount { get; set; }

		public string Id { get; set; }

		public int Quantity { get; set; }

		public string Checkin { get; set; }

		public List<string> AccountCodes { get; set; }

		public decimal Discount { get; set; }

		public string PromotionID { get; set; }

		public string PromotionLinkID { get; set; }

		public Guid? ShiftID { get; set; }

		public Guid? _ZoneID { get; set; }

		public Guid? ZoneGroupID { get; set; }

		public List<LockSeat> listSeatLock { get; set; }

		public BookingDetailReq()
		{
			AccountCodes = new List<string>();
			listSeatLock = new List<LockSeat>();
		}
	}
}
