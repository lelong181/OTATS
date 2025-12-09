using System;

namespace Model.ResponseModel{

public class ServiceRateRepay
{
	public Guid? InvoiceID { get; set; }

	public string InvoiceCode { get; set; }

	public Guid ServiceRateID { get; set; }

	public decimal? Price { get; set; }

	public int Quantity { get; set; }

	public decimal? Amount { get; set; }

	public int RepayQuantity { get; set; }

	public string ServiceRateName { get; set; }

	public string PromotionName { get; set; }

	public Guid? PromotionID { get; set; }

	public Guid? CouponID { get; set; }

	public bool DiscountOnBill { get; set; }
}
}