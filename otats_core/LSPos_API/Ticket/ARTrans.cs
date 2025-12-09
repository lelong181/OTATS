using System;

namespace Ticket
{
	public class ARTrans
	{
		public Guid ID { get; set; }

		public Guid? AccountReceivableID { get; set; }

		public DateTime? TransactionDate { get; set; }

		public Guid? BookingID { get; set; }

		public string BookingCode { get; set; }

		public decimal TotalAmount { get; set; }

		public decimal ReMainingAmount { get; set; }

		public Guid ServiceID { get; set; }

		public string ServiceCode { get; set; }

		public string ServiceTitle { get; set; }

		public decimal AllocatedAmount { get; set; }

		public string Description { get; set; }

		public string Status { get; set; }

		public string UserFullName { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }
	}
}
