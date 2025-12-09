using System;

namespace BusinessLayer.Model.API
{
	public class BookingPaymentDetailInfoResponse
	{
		public Guid? InvoicePaymentTypeID { get; set; }

		public bool? IsDeposit { get; set; }

		public string PaymentTypeName { get; set; }

		public string PaymentTypeCode { get; set; }

		public decimal? Amount { get; set; }
	}
}
