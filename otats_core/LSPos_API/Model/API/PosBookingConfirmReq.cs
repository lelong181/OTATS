using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class PosBookingConfirmReq
	{
		public string BookingCode { get; set; }

		public decimal PrepayAmount { get; set; }

		public decimal ReturnAmount { get; set; }

		public string Note { get; set; }

		public List<PaymentTypeRequest> PaymentTypes { get; set; }

		public string SiteId { get; set; }

		public string SessionId { get; set; }

		public PosBookingConfirmReq()
		{
			PaymentTypes = new List<PaymentTypeRequest>();
		}
	}
}
