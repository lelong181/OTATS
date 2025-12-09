using System;

namespace BusinessLayer.Model.API
{
	public class BookingB2bDetail
	{
		public Guid Booking_ID { get; set; }

		public string BookingCode { get; set; }

		public DateTime CheckinDate { get; set; }

		public DateTime CreatedDate { get; set; }

		public decimal TotalAmount { get; set; }

		public string CustomerName { get; set; }

		public string BookingStatus { get; set; }

		public string BookingStatusStr { get; set; }

		public string PhoneNumber { get; set; }

		public string Email { get; set; }

		public string InvoiceCode { get; set; }

		public Guid? InvoiceId { get; set; }

		public string OrderCode { get; set; }

		public DateTime BookingDate { get; set; }

		public string BookingUser { get; set; }

		public string TaxCode { get; set; }

		public string ProfileAddress { get; set; }

		public string CustomerID { get; set; }

		public int TotalQuantity { get; set; }

		public decimal? TotalBookingPayment { get; set; }
	}
}
