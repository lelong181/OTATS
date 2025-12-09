using System;

namespace Model.ResponseModel{

public class BookingAccountStatusDetail
{
	public string BookingCode { get; set; }

	public DateTime? CheckinDate { get; set; }

	public string Notes { get; set; }

	public string OrderCode { get; set; }

	public string BookingStatus { get; set; }

	public string AccountCode { get; set; }

	public string Status { get; set; }

	public DateTime? IssuedDate { get; set; }

	public DateTime? ExpirationDate { get; set; }

	public string ServiceRateName { get; set; }

	public string ServiceName { get; set; }

	public DateTime? SaleDate { get; set; }
}
}