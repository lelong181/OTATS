using System;

namespace Model.ResponseModel{

public class InvoicePrintTaxResponse
{
	public string SerialNumber { get; set; }

	public decimal Price { get; set; }

	public string PriceText { get; set; }

	public string InvoiceCode { get; set; }

	public string AccountCode { get; set; }

	public string ServiceName { get; set; }

	public string FlexCol1 { get; set; }

	public Guid ServiceID { get; set; }

	public DateTime? CreatedDate { get; set; }
}
}