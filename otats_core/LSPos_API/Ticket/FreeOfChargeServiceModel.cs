using System;

namespace Ticket
{
	public class FreeOfChargeServiceModel : BaseModel
	{
		public int ID { get; set; }

		public int FreeOfChargeID { get; set; }

		public int ServiceID { get; set; }

		public int Quantity { get; set; }

		public decimal DiscountRate { get; set; }

		public string Description { get; set; }

		public bool Inactive { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }
	}
}
