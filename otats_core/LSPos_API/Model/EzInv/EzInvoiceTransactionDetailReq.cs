using System;

namespace BusinessLayer.Model.EzInv
{
	public class EzInvoiceTransactionDetailReq
	{
		public Guid ID { get; set; }

		public Guid? EzInvoice_TransactionID { get; set; }

		public string ItemCode { get; set; }

		public string ItemName { get; set; }

		public string Unit { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Amount { get; set; }

		public decimal TaxRate { get; set; }

		public decimal TaxAmount { get; set; }

		public decimal TotalAmount { get; set; }

		public string Notes { get; set; }

		public Guid? BookingDetailID { get; set; }

		public Guid? ShiftID { get; set; }

		public string InvoiceCode { get; set; }
	}
}
