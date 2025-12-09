using System;

namespace Ticket
{
	public class ServicePriceModel : BaseModel
	{
		public int ID { get; set; }

		public int ServiceID { get; set; }

		public int DefineDateID { get; set; }

		public DateTime BeginDate { get; set; }

		public DateTime EndDate { get; set; }

		public decimal Price { get; set; }

		public bool Inactive { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }
	}
}
