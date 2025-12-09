using System;
using System.Collections.Generic;

namespace Ticket
{
	public class FreeOfChargeModel : BaseModel
	{
		public int ID { get; set; }

		public int Type { get; set; }

		public int ServiceID { get; set; }

		public int ProfileID { get; set; }

		public int Quantity { get; set; }

		public int TotalMoney { get; set; }

		public int FocType { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string Description { get; set; }

		public bool Inactive { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }

		public List<FreeOfChargeServiceModel> ListFreeOfChargeServiceModel { get; set; }

		public FreeOfChargeModel()
		{
			ListFreeOfChargeServiceModel = new List<FreeOfChargeServiceModel>();
		}
	}
}
