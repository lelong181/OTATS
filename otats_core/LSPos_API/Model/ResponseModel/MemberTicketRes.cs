using System;

namespace Model.ResponseModel{

public class MemberTicketRes
{
	public string ServiceRateName { get; set; }

	public string AccountCode { get; set; }

	public DateTime? IssuedDate { get; set; }

	public DateTime? ExpirationDate { get; set; }

	public string Status { get; set; }

	public string StatusStr { get; set; }

	public string ServiceName { get; set; }

	public string InvoiceCode { get; set; }

	public DateTime? InvoiceDate { get; set; }

	public string BookingCode { get; set; }

	public DateTime? CreatedDate { get; set; }

	public int? NumberUsing { get; set; }
}
}