using System;

namespace Model.ResponseModel.BiInterface{

public class BitbPaymentRes
{
	public Guid PaymentId { get; set; }

	public Guid TransactionId { get; set; }

	public int PaymentStatus { get; set; }

	public decimal PaymentAmount { get; set; }

	public Guid PluginId { get; set; }

	public string PlugInName { get; set; }

	public Guid? AccountId { get; set; }

	public bool Change { get; set; }

	public int PaymentCount { get; set; }

	public Guid CategoryId { get; set; }
}
}