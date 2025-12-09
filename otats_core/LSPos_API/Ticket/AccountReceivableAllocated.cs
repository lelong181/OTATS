using System;

namespace Ticket
{
	public class AccountReceivableAllocated
	{
		public Guid ID { get; set; }

		public Guid FromAccountReceivableTransID { get; set; }

		public Guid ToAccountReceivableTransID { get; set; }

		public decimal Amount { get; set; }

		public DateTime AllocatedDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public bool HasChange { get; set; }

		public bool Inactive { get; set; }
	}
}
