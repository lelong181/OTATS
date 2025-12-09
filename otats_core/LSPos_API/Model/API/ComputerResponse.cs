using System;

namespace BusinessLayer.Model.API
{
	public class ComputerResponse
	{
		public Guid ID { get; set; }

		public string Name { get; set; }

		public string IPAddress { get; set; }

		public string PhysicalAddress { get; set; }

		public Guid? ComputerTypeID { get; set; }

		public Guid? ZoneID { get; set; }

		public string Description { get; set; }

		public bool IsACM { get; set; }

		public bool Inactive { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }

		public virtual ComputerType ComputerType { get; set; }
	}
}
