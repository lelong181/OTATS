using System;

namespace BusinessLayer.Model
{
	public class LockSeat
	{
		public Guid ShiftSeatID { get; set; }

		public Guid SeatID { get; set; }

		public bool hasSetSeat { get; set; } = false;

	}
}
