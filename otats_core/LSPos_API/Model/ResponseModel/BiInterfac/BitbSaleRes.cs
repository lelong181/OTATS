using System;

namespace Model.ResponseModel.BiInterface{

public class BitbSaleRes
{
	public Guid SaleID { get; set; }

	public Guid? UserAccountId { get; set; }

	public Guid WorkstationId { get; set; }

	public int SaleStatus { get; set; }

	public DateTime SaleDateTime { get; set; }

	public DateTime SaleFiscalDate { get; set; }

	public string SaleCode { get; set; }

	public bool Approved { get; set; }

	public bool Paid { get; set; }

	public bool Encoded { get; set; }

	public bool Printed { get; set; }

	public bool Validated { get; set; }

	public bool Completed { get; set; }

	public decimal TotalAmount { get; set; }

	public decimal PaidAmount { get; set; }

	public int ItemCount { get; set; }

	public Guid? HoldId { get; set; }

	public string ReceiptEmailAddress { get; set; }

	public bool AutoPurge { get; set; }

	public Guid? SaleChannelId { get; set; }

	public int SaleFlowType { get; set; }

	public string BatchDate { get; set; }

	public int BatchNumber { get; set; }

	public bool TaxExempt { get; set; }

	public Guid SaleToAccountId { get; set; }
}
}