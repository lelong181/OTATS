using System;

namespace Model.ResponseModel.BiInterface{

public class BitbTransactionRes
{
	public Guid TransactionId { get; set; }

	public Guid SaleId { get; set; }

	public Guid WorkstationId { get; set; }

	public Guid? UserAccountId { get; set; }

	public int TransactionType { get; set; }

	public DateTime TransactionDateTime { get; set; }

	public DateTime TransactionFiscalDate { get; set; }

	public int TransactionSerial { get; set; }

	public int ItemCount { get; set; }

	public decimal TotalAmount { get; set; }

	public decimal TotalTax { get; set; }

	public decimal PaidAmount { get; set; }

	public decimal PaidTax { get; set; }

	public bool Approved { get; set; }

	public bool Paid { get; set; }

	public bool Encoded { get; set; }

	public bool Printed { get; set; }

	public bool Validated { get; set; }

	public Guid? BoxId { get; set; }

	public int PrintedCount { get; set; }

	public Guid? SupAccountId { get; set; }

	public int? StationSerial { get; set; }

	public int? DurationSelection { get; set; }

	public int? DurationPayment { get; set; }

	public int? DurationPrint { get; set; }

	public DateTime? SerialFiscalDate { get; set; }

	public DateTime? SerialDateTime { get; set; }
}
}