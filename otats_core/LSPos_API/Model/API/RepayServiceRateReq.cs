using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class RepayServiceRateReq
	{
		public bool RepayFullBooking { get; set; }

		public Guid BookingID { get; set; }

		public Guid? WalletID { get; set; }

		public string WalletCode { get; set; }

		public string BookingCode { get; set; }

		public Guid? InvoiceID { get; set; }

		public string CreatedBy { get; set; }

		public Guid SessionID { get; set; }

		public string Note { get; set; }

		public decimal TotalAmountRepay { get; set; }

		public string ComputerID { get; set; }

		public string ZoneID { get; set; }

		public string Channel { get; set; }

		public string SiteCode { get; set; }

		public Guid SiteID { get; set; }

		public Guid? ShiftID { get; set; }

		public List<RepayPaymentTypeReq> PaymentTypes { get; set; }

		public List<RepayServiceRateDetailReq> Details { get; set; }

		public RepayServiceRateReq()
		{
			PaymentTypes = new List<RepayPaymentTypeReq>();
			Details = new List<RepayServiceRateDetailReq>();
		}
	}
}
