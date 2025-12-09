using System;

namespace Ticket
{
	public class ServiceTypeModel : BaseModel
	{
		public int ID { get; set; }

		public int Type { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public bool Inactive { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }
	}
}
