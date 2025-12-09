using System;

namespace BusinessLayer.Model.API
{
	public class SeatAvailableRes
	{
		public Guid ShiftID { get; set; }

		public string SeatCode { get; set; }

		public string SeatNumber { get; set; }

		public string SeatNumberStr { get; set; }

		public string SeatType { get; set; }

		public int? SeatSortIndex { get; set; }

		public int? ZoneGroupSortIndex { get; set; }

		public string ZoneGroupName { get; set; }

		public Guid ZoneID { get; set; }

		public Guid ZoneGroupID { get; set; }

		public string Status { get; set; }

		public string StatusStr { get; set; }

		public Guid ShiftSeatID { get; set; }

		public Guid SeatID { get; set; }

		public bool IsSelected { get; set; }

		public string ServiceRateID { get; set; }

		public string ServiceRateName { get; set; }

		public string ZoneName { get; set; }

		public decimal Price { get; set; }
	}
}
