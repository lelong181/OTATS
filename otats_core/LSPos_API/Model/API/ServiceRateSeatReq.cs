using System;

namespace BusinessLayer.Model.API
{
	public class ServiceRateSeatReq
	{
		public Guid ZoneGroupID { get; set; }

		public Guid ShiftID { get; set; }

		public DateTime DateSell { get; set; }
	}
}
