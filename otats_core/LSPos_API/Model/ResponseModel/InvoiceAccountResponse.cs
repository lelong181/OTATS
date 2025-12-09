using System;

namespace Model.ResponseModel{

public class InvoiceAccountResponse
{
	public Guid AccountID { get; set; }

	public string ServiceRateName { get; set; }

	public string ServiceName { get; set; }

	public DateTime? IssuedDate { get; set; }

	public DateTime? ExpirationDate { get; set; }

	public decimal TotalMoney { get; set; }

	public string Status { get; set; }

	public string StatusStr { get; set; }

	public int? PrintCount { get; set; }

	public string AccountCode { get; set; }

	public int? Sequence { get; set; }

	public string CardType { get; set; }

	public string CardTypeStr { get; set; }

	public Guid? MemberID { get; set; }

	public string MemberFullName { get; set; }

	public string MemberGender { get; set; }

	public string MemberAddress { get; set; }

	public string MemberEmail { get; set; }

	public string MemberPhoneNumber { get; set; }

	public string MemberIdentityCard { get; set; }

	public int? NumberUsing { get; set; }

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