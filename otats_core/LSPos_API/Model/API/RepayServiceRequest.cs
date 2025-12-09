using System;
using System.Collections.Generic;
using Ticket;

namespace BusinessLayer.Model.API
{
	public class RepayServiceRequest
	{
		public bool RepayFullBooking { get; set; }

		public Guid SessionId { get; set; }

		public string SiteCode { get; set; }

		public Guid SiteId { get; set; }

		public string ComputerId { get; set; }

		public string ZoneId { get; set; }

		public string Channel { get; set; }

		public string CreatedBy { get; set; }

		public decimal TotalAmount { get; set; }

		public string ProfileId { get; set; }

		public string InvoiceId { get; set; }

		public string Note { get; set; }

		public CancellationRuleResponse CancelRule { get; set; }

		public List<PaymentTypeRequest> PaymentTypes { get; set; }

		public List<ServiceSelectedModel> ServiceRepay { get; set; }

		public List<BookingDetailReq> Details { get; set; }

		public RepayServiceRequest()
		{
			PaymentTypes = new List<PaymentTypeRequest>();
			ServiceRepay = new List<ServiceSelectedModel>();
			Details = new List<BookingDetailReq>();
		}

		public void Clear()
		{
			InvoiceId = "";
			Note = "";
			PaymentTypes.Clear();
			ServiceRepay.Clear();
			Details.Clear();
			CancelRule = new CancellationRuleResponse();
		}

		public void SetBookingDetail()
		{
			Details.Clear();
			foreach (ServiceSelectedModel service in ServiceRepay)
			{
				Details.Add(new BookingDetailReq
				{
					Id = service.ServiceRateID,
					Amount = service.Amount,
					Checkin = DateTime.Now.ToString("yyyy-MM-dd"),
					Quantity = service.Quantity,
					AccountCodes = service.AccountCodes
				});
			}
		}
	}
}
