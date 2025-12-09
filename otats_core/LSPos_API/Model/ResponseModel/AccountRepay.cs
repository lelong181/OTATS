using System;

namespace Model.ResponseModel{

public class AccountRepay
{
	public Guid AccountID { get; set; }

	public string CardID { get; set; }

	public string AccountCode { get; set; }

	public DateTime? IssuedDate { get; set; }

	public DateTime? ExpirationDate { get; set; }

	public decimal? TotalMoney { get; set; }

	public string CardType { get; set; }

	public Guid? ServiceID { get; set; }

	public string ServiceName { get; set; }

	public Guid? ServiceRateID { get; set; }

	public string Status { get; set; }

	public string StatusStr { get; set; }

	public string ServiceRateName { get; set; }

	public Guid? BookingDetailSeatID { get; set; }

	public Guid? ShiftID { get; set; }

	public string ShiftName { get; set; }

	public DateTime? ShiftFromDate { get; set; }

	public string ZoneName { get; set; }

	public string ZoneGroupName { get; set; }

	public string SeatCode { get; set; }

	public string SeatNumber { get; set; }

	public string SeatType { get; set; }

	public string SerialNo { get; set; }
}
}