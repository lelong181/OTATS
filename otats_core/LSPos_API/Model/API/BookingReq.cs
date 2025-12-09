using System;
using System.Collections.Generic;
using BusinessLayer.Model.Sell;

namespace BusinessLayer.Model.API
{
	public class BookingReq
	{
		public string Checkin { get; set; }

		public string MemberId { get; set; }

		public string ProfileId { get; set; }

		public string SiteId { get; set; }

		public string SiteCode { get; set; }

		public string Lang { get; set; }

		public string Note { get; set; }

		public string SessionId { get; set; }

		public string Channel { get; set; }

		public string CreatedBy { get; set; }

		public decimal PaidAmount { get; set; }

		public decimal ReturnAmount { get; set; }

		public string ComputerId { get; set; }

		public string ZoneId { get; set; }

		public string BookingCode { get; set; }

		public string OrderCode { get; set; }

		public bool IsExportTicket { get; set; }

		public string EmailTo { get; set; }

		public string EmailCC { get; set; }

		public string EmailBCC { get; set; }

		public List<CustomerBookingReq> BookingCustomers { get; set; }

		public List<CustomerB2BSelectedModel> Customers { get; set; }

		public List<BookingDetailReq> Details { get; set; }

		public List<PaymentTypeRequest> PaymentTypes { get; set; }

		public List<SellCardInput> listSellCard { get; set; }

		public Guid? ShiftID { get; set; }

		public BookingReq()
		{
			BookingCustomers = new List<CustomerBookingReq>();
			Details = new List<BookingDetailReq>();
			PaymentTypes = new List<PaymentTypeRequest>();
			Customers = new List<CustomerB2BSelectedModel>();
			listSellCard = new List<SellCardInput>();
		}
	}
}
