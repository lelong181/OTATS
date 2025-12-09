using System;

namespace Model.ResponseModel.BiInterface{

public class BitbTransactionItemRes
{
	public Guid TransactionId { get; set; }

	public Guid SaleItemId { get; set; }

	public int Quantity { get; set; }

	public int QuantityPaid { get; set; }
}
}