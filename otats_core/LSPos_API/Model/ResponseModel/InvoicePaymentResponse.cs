using System;

namespace Model.ResponseModel
{

	public class InvoicePaymentResponse
	{
		public Guid? InvoicePaymentTypeID { get; set; }

		public bool? IsDeposit { get; set; }

		public string PaymentTypeName { get; set; }

		public string PaymentTypeCode { get; set; }

		public decimal? Amount { get; set; }

		public string Cashier { get; set; }

		public DateTime? PaymentDate { get; set; }
	}
}