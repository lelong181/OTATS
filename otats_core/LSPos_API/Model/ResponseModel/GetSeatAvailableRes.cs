using System;

namespace Model.ResponseModel{

public class GetSeatAvailableRes
{
	public Guid ShiftSeatID { get; set; }

	public Guid ShiftID { get; set; }

	public DateTime? ShiftFromDate { get; set; }

	public string SeatCode { get; set; }

	public string SeatNumber { get; set; }

	public string SeatType { get; set; }

	public Guid? ShiftTypeID { get; set; }

	public string ZoneName { get; set; }

	public string ZoneGroupName { get; set; }

	public Guid ZoneGroupID { get; set; }

	public bool? IsHideOnTicket { get; set; }
}
}