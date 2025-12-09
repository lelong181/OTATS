using System;

namespace BusinessLayer.Model.API
{
	public class DetailRepayServiceRateReq
	{
		public Guid? InvoiceID { get; set; }

		public Guid? BookingID { get; set; }

		public Guid? WalletID { get; set; }

		public string BookingCode { get; set; }

		public string LangCode { get; set; }
	}
}
