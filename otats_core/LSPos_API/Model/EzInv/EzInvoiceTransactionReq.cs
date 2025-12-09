using System;

namespace BusinessLayer.Model.EzInv
{
	public class EzInvoiceTransactionReq
	{
		public Guid ID { get; set; }

		public Guid? EzInvoiceConfigurationID { get; set; }

		public Guid? EzInvoice_InvoiceID { get; set; }

		public string EzInvoice_ProviderInvoiceID { get; set; }

		public string EzInvoice_CMCInvoiceID { get; set; }

		public string FormNo { get; set; }

		public string Serial { get; set; }

		public DateTime InvoiceDate { get; set; }

		public string CustomerCode { get; set; }

		public string CustomerName { get; set; }

		public string CustomerPhone { get; set; }

		public string CustomerTax { get; set; }

		public string CustomerAddress { get; set; }

		public string CustomerEmail { get; set; }

		public string CustomerSave { get; set; }

		public string BankAccount { get; set; }

		public string BankName { get; set; }

		public string CurrencyCode { get; set; }

		public string ExchangeRate { get; set; }

		public string PaymentMethod { get; set; }

		public decimal TotalAmount { get; set; }

		public DateTime? Due { get; set; }

		public decimal DiscountPercent { get; set; }

		public decimal DiscountAmount { get; set; }

		public decimal Charge { get; set; }

		public string Description { get; set; }

		public DateTime? SignDate { get; set; }

		public DateTime? CreatedDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? UpdateDate { get; set; }

		public string UpdateBy { get; set; }

		public EzInvoiceTransactionReq()
		{
			TotalAmount = 0m;
			DiscountPercent = 0m;
			DiscountAmount = 0m;
			Charge = 0m;
		}
	}
}
