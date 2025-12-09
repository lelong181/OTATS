using System;

namespace Model.ResponseModel{

public class ServiceRateSeatRes
{
	public Guid ShiftID { get; set; }

	public Guid ZoneGroupID { get; set; }

	public Guid ServiceRateID { get; set; }

	public int SeatAvailable { get; set; }
}
}