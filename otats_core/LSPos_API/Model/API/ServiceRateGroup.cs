using System;

namespace BusinessLayer.Model.API
{
	public class ServiceRateGroup
	{
		public Guid ServiceRateGroupID { get; set; }

		public string GroupCode { get; set; }

		public int? Sequence { get; set; }

		public string ServiceRateGroupName { get; set; }
	}
}
