using System;

namespace Model.ResponseModel{

public class KiosCheckinResponse
{
	public Guid AccountID { get; set; }

	public string ServiceName { get; set; }

	public DateTime UsingDate { get; set; }

	public string ServiceCode { get; set; }

	public int PrintCount { get; set; }

	public string Status { get; set; }

	public int Sequence { get; set; }

	public string CurrencyCode { get; set; }

	public string Notes { get; set; }

	public string BookingStatus { get; set; }

	public string Channel { get; set; }

	public string Classify { get; set; }

	public byte[] Image { get; set; }

	public string InvoiceCode { get; set; }

	public Guid InvoiceID { get; set; }

	public decimal TotalMoney { get; set; }
}
}