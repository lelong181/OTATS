namespace BusinessLayer.Model.EzInv
{
	public class EzInvoiceTempExport : EzInvoiceTransactionReq
	{
		public int STT { get; set; }

		public string ItemCode { get; set; }

		public string ItemName { get; set; }

		public string Unit { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Amount { get; set; }

		public decimal TaxRate { get; set; }

		public decimal TaxAmount { get; set; }

		public decimal Total { get; set; }

		public string InvoiceCode { get; set; }
	}
}
