using System;

namespace Model.ResponseModel{

public class RptCouponTrans
{
	public string StatusText { get; set; }

	public string Code { get; set; }

	public string BookingCode { get; set; }

	public string InvoiceCode { get; set; }

	public decimal CouponAmount { get; set; }

	public decimal BookingAmount { get; set; }

	public decimal InvoiceAmount { get; set; }

	public DateTime? SaleDate { get; set; }

	public DateTime? BeginDate { get; set; }

	public DateTime? EndDate { get; set; }
}
}