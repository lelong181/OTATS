using System;

namespace Model.ResponseModel{

public class SeatAvailableRes
{
	public Guid ShiftID { get; set; }

	public string SeatCode { get; set; }

	public string SeatNumber { get; set; }

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

	public Guid? ShiftTypeID { get; set; }

	public string ZoneName { get; set; }

	public DateTime? FromDate { get; set; }

	public DateTime? CheckInDate { get; set; }

	public DateTime? ToDate { get; set; }

	public bool? IsHideOnTicket { get; set; }

	public bool? AnotherSeatType { get; set; }

	public Guid? ServiceRateID { get; set; }
}
}