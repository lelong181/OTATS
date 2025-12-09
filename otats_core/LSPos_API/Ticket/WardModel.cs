using System;

namespace Ticket
{
	public class WardModel : BaseModel
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public int DistrictID { get; set; }

		public bool Inactive { get; set; }

		public DateTime CreatedDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }

		public string UpdatedBy { get; set; }
	}
}
