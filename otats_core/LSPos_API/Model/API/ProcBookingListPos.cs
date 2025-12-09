using System;

namespace BusinessLayer.Model.API
{
	public class ProcBookingListPos
	{
		public Guid ID { get; set; }

		public string BookingCode { get; set; }

		public DateTime CheckinDate { get; set; }

		public DateTime CreatedDate { get; set; }

		public decimal TotalAmount { get; set; }

		public string CustomerName { get; set; }

		public string BookingStatus { get; set; }

		public string BookingStatusStr { get; set; }

		public string IdOrPPNum { get; set; }

		public string PhoneNumber { get; set; }

		public string Email { get; set; }

		public string InvoiceCode { get; set; }

		public Guid? InvoiceId { get; set; }

		public string OrderCode { get; set; }

		public string CustomerType { get; set; }

		public decimal? TotalBookingPayment { get; set; }

		public string BookingUser { get; set; }
	}
}
