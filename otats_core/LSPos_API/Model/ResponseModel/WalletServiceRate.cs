using System;

namespace Model.ResponseModel{

public class WalletServiceRate
{
	public string ServiceRateName { get; set; }

	public Guid ServiceID { get; set; }

	public decimal? Price { get; set; }

	public DateTime? SaleDate { get; set; }

	public decimal? TotalAmount { get; set; }

	public int? Quantity { get; set; }

	public int? UnitQuantity { get; set; }

	public Guid? ServicePackageID { get; set; }

	public bool? PrintTicketBy { get; set; }

	public decimal? PublicNetPrice { get; set; }

	public Guid? ServiceRateID { get; set; }

	public string Status { get; set; }

	public string GroupBy { get; set; }

	public string TransactionNo { get; set; }

	public string Description { get; set; }

	public Guid? PromotionID { get; set; }

	public Guid? BookingDetailID { get; set; }

	public Guid? InvoiceID { get; set; }

	public string ServiceName { get; set; }
}
}