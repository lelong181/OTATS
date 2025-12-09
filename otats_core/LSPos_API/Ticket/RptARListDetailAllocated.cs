using System;

namespace Ticket
{
	public class RptARListDetailAllocated
	{
		public Guid? FromAccountReceivableTransID { get; set; }

		public Guid? ToAccountReceivableTransID { get; set; }

		public decimal? AllocatedAmountDetail { get; set; }

		public DateTime? AllocatedDateDetail { get; set; }

		public string ServiceDetailName { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public string BookingCode { get; set; }
	}
}
