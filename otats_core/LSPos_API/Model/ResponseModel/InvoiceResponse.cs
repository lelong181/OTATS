using System;

namespace Model.ResponseModel{

public class InvoiceResponse
{
	public Guid ID { get; set; }

	public string InvoiceCode { get; set; }

	public string SiteCode { get; set; }

	public string Description { get; set; }

	public string CreatedBy { get; set; }

	public string UpdatedBy { get; set; }

	public DateTime InvoiceDate { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime UpdatedDate { get; set; }

	public decimal? TotalMoney { get; set; }

	public decimal Amount { get; set; }

	public decimal DiscountAmount { get; set; }

	public decimal PaidAmount { get; set; }

	public decimal ReturnAmount { get; set; }

	public Guid? SessionID { get; set; }

	public int SessionNo { get; set; }

	public bool Inactive { get; set; }

	public int PrintCount { get; set; }

	public int PrintTaxCount { get; set; }

	public DateTime UsingDate { get; set; }

	public Guid? ProfileID { get; set; }

	public string InvoicePaymentType { get; set; }

	public string Cashier { get; set; }

	public string Channel { get; set; }
}
}