using System;

namespace Model.ResponseModel{

public class AccountTicketRes
{
	public Guid AccountID { get; set; }

	public string AccountCode { get; set; }

	public decimal? TotalMoney { get; set; }

	public DateTime? IssuedDate { get; set; }

	public DateTime? ExpirationDate { get; set; }

	public string ServiceName { get; set; }

	public string ServiceRateName { get; set; }

	public Guid InvoiceID { get; set; }

	public Guid ServiceID { get; set; }

	public Guid ServiceRateID { get; set; }

	public Guid ServicePackageID { get; set; }

	public string InvoiceCode { get; set; }

	public DateTime? InvoiceDate { get; set; }

	public int? SessionNo { get; set; }

	public string CashierFullName { get; set; }

	public string CashierUserName { get; set; }

	public string BookingCode { get; set; }

	public string BookingStatus { get; set; }

	public string Status { get; set; }

	public string StatusStr { get; set; }

	public string Channel { get; set; }

	public string Source { get; set; }
}
}