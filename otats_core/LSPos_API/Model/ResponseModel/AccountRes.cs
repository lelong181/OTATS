using System;

namespace Model.ResponseModel{

public class AccountRes
{
	public Guid AccountID { get; set; }

	public string AccountCode { get; set; }

	public DateTime IssuedDate { get; set; }

	public DateTime ExpirationDate { get; set; }

	public Guid ServicePackageID { get; set; }

	public Guid ServiceRateID { get; set; }

	public string Title { get; set; }

	public Guid InvoiceID { get; set; }

	public string Status { get; set; }

	public string StatusStr { get; set; }

	public string InvoiceCode { get; set; }

	public string BookingCode { get; set; }
}
}