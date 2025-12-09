using System;

namespace BusinessLayer.Model.API
{
	public class ComputerType
	{
		public Guid ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public bool Inactive { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }
	}
}
