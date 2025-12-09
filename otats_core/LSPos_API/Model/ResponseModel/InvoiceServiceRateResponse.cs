using System;

namespace Model.ResponseModel{

public class InvoiceServiceRateResponse
{
	public Guid? ServiceRateID { get; set; }

	public string ServiceRateName { get; set; }

	public string PromotionName { get; set; }

	public Guid? PromotionID { get; set; }

	public string CouponName { get; set; }

	public Guid? CouponID { get; set; }

	public int? Quantity { get; set; }

	public decimal? Price { get; set; }

	public decimal? Amount { get; set; }

	public DateTime? SaleDate { get; set; }
}
}