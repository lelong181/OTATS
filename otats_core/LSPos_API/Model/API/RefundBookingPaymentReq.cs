using System;

namespace BusinessLayer.Model.API
{
	public class RefundBookingPaymentReq
	{
		public Guid BookingID { get; set; }

		public Guid BookingPaymentID { get; set; }

		public decimal AmountRefund { get; set; }

		public decimal Amount { get; set; }

		public Guid ServiceID { get; set; }

		public string CreatedBy { get; set; }

		public string Note { get; set; }

		public string SessionId { get; set; }
	}
}
