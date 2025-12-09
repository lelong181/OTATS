using System;
using Ticket.Business;
using Ticket.Utils;

namespace Ticket
{
	public class CustomerWonVoucherModel : BaseModel
	{
		public int ID { get; set; }

		public string Fullname { get; set; }

		public string Gender { get; set; }

		public DateTime DateOfBirth { get; set; }

		public string IdentityNo { get; set; }

		public string Job { get; set; }

		public string Address { get; set; }

		public string District { get; set; }

		public string City { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public int ServiceID { get; set; }

		public string ServiceName { get; set; }

		public int Quantity { get; set; }

		public DateTime CanReceiveFromDate { get; set; }

		public DateTime CanReceiveToDate { get; set; }

		public string ReferenceCode { get; set; }

		public bool VoucherReceived { get; set; }

		public DateTime ReceiveDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }

	}
}
