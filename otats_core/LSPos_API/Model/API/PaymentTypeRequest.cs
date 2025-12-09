using System;

namespace BusinessLayer.Model.API
{
	public class PaymentTypeRequest
	{
		public Guid PaymentID { get; set; }

		public Guid? BookingPaymentID { get; set; }

		public string Name { get; set; }

		public string PaymentTypeName { get; set; }

		public decimal Amount { get; set; }

		public decimal InputAmount { get; set; }

		public bool IsNewPayment { get; set; }

		public VoucherRes Voucher { get; set; }

		public bool? IsPaymentDeposit { get; set; } = false;

	}
}
