using System;
using System.Collections.Generic;
using BusinessLayer.Model.Sell;

namespace BusinessLayer.Model.API
{
	public class BookingRes
	{
		public Guid? BookingID { get; set; }

		public string BookingCode { get; set; }

		public string Channel { get; set; }

		public string Source { get; set; }

		public string CurrencyCode { get; set; }

		public string CheckinDate { get; set; }

		public DateTime CheckinDateDt { get; set; }

		public string Lang { get; set; }

		public string StatusCode { get; set; }

		public DepositRes Deposit { get; set; }

		public CancelRes Cancel { get; set; }

		public string CreatedDate { get; set; }

		public decimal? TotalAmount { get; set; }

		public string BookingStatus { get; set; }

		public string Notes { get; set; }

		public ProfileSelectedModel Profile { get; set; }

		public List<BookingAccountResponse> BookingAccounts { get; set; }

		public List<BookingAccountResponse> Account { get; set; }

		public List<BookingPaymentRes> BookingPayments { get; set; }

		public List<CustomerRes> CustomerRes { get; set; }

		public List<BookingRateRes> Rates { get; set; }

		public List<ServiceRes> ServiceRes { get; set; }

		public static DateTime CheckInDate { get; set; }

		public string CheckinDateStr { get; set; }

		public string Name { get; set; }

		public string IdOrPPNum { get; set; }

		public string PhoneNumber { get; set; }

		public string Email { get; set; }

		public string PaymentTypeSummary { get; set; }
	}
}
