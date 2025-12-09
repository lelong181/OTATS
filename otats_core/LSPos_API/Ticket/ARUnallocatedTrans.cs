using System;

namespace Ticket
{
	public class ARUnallocatedTrans
	{
		public Guid arTransID { get; set; }

		public Guid? arTransBookingID { get; set; }

		public string CreatedBy { get; set; }

		public Guid AccountReceivableID { get; set; }
	}
}
