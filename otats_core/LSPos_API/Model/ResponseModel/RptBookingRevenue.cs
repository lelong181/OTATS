using System;

namespace Model.ResponseModel{

public class RptBookingRevenue
{
	public string BookingCode { get; set; }

	public string ProfileCode { get; set; }

	public string ProfileName { get; set; }

	public string BookingStatus { get; set; }

	public int Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Amount { get; set; }

	public decimal DepositAmount { get; set; }

	public decimal PaymentAmount { get; set; }

	public DateTime? CheckinDate { get; set; }

	public DateTime? CreatedDate { get; set; }
}
}