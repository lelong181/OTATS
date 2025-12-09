using System;

namespace Ticket
{
	public class AppSettings
	{
		public Guid ID { get; set; }

		public Guid ComputerID { get; set; }

		public string Invoice { get; set; }

		public string PaymentTypeID { get; set; }

		public string SiteCode { get; set; }

		public string ServiceSell { get; set; }

		public string TicketTemplateQR { get; set; }

		public Guid? ServiceWalletID { get; set; }

		public Guid? ServiceRateWalletCheckoutID { get; set; }

		public int? NoOfBill { get; set; }

		public int? NoOfBillReprint { get; set; }

		public string PrinterQrName { get; set; }

		public string PrinterBillName { get; set; }

		public int? NoOfQr { get; set; }

		public int? NoOfQrReprint { get; set; }
	}
}
