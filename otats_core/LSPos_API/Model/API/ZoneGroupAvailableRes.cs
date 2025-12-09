using System;

namespace BusinessLayer.Model.API
{
	public class ZoneGroupAvailableRes
	{
		public Guid ShiftID { get; set; }

		public Guid ZoneID { get; set; }

		public Guid ZoneGroupID { get; set; }

		public string ZoneGroupName { get; set; }

		public string ZoneName { get; set; }

		public int? SeatAvailable { get; set; }

		public Guid ServiceRateID { get; set; }
	}
}
