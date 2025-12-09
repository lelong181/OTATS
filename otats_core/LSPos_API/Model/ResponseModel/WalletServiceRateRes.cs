using System;
using System.Collections.Generic;

namespace Model.ResponseModel{

public class WalletServiceRateRes
{
	public Guid? ServiceRateID { get; set; }

	public string ServiceRateName { get; set; }

	public string ServiceName { get; set; }

	public DateTime? SaleDate { get; set; }

	public decimal? Price { get; set; }

	public int? Quantity { get; set; }

	public int? UnitQuantity { get; set; }

	public decimal Discount { get; set; }

	public decimal TotalAmount { get; set; }

	public string Status { get; set; }

	public string Description { get; set; }

	public List<Guid> ListService { get; set; }

	public Guid? BookingDetailID { get; set; }

	public WalletServiceRateRes()
	{
		ListService = new List<Guid>();
	}
}
}