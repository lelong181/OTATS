using System;

namespace BusinessLayer.Model.API
{
	public class BookingPaymentRes
	{
		public string PaidDate { get; set; }

		public decimal Amount { get; set; }

		public string ResCode { get; set; }

		public string ResMsg { get; set; }

		public string ResponseCode { get; set; }

		public string ResponseMessage { get; set; }

		public PaymentMethodResponse PaymentMethod { get; set; }

		public string ServiceName { get; set; }

		public Guid ServiceID { get; set; }

		public Guid BookingPaymentID { get; set; }

		public Guid? OriginBookingPaymentID { get; set; }

		public bool? IsRefundPayment { get; set; }
	}
}
