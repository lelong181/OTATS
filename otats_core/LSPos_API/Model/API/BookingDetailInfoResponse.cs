using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class BookingDetailInfoResponse
	{
		public List<BookingPaymentDetailInfoResponse> ListPaymentType { get; set; }

		public List<BookingAccountInfoResponse> ListAccount { get; set; }

		public List<BookingServiceRateInfoResponse> ListServiceRate { get; set; }

		public List<CustomerBookingDetailInfoResponse> ListCustomer { get; set; }

		public BookingDetailInfoResponse()
		{
			ListPaymentType = new List<BookingPaymentDetailInfoResponse>();
			ListAccount = new List<BookingAccountInfoResponse>();
			ListServiceRate = new List<BookingServiceRateInfoResponse>();
			ListCustomer = new List<CustomerBookingDetailInfoResponse>();
		}
	}
}
