using System;

namespace BusinessLayer.Model.API
{
	public class RptDetailBookingUpgradeRes
	{
		public DateTime? UpgradeTime { get; set; }

		public string Cashier { get; set; }

		public string BookingCode { get; set; }

		public string InvoiceCode { get; set; }

		public string ComputerName { get; set; }

		public string OriginServiceRateName { get; set; }

		public string OriginAccount { get; set; }

		public string UpgradeAccount { get; set; }

		public string OriginStatus { get; set; }

		public string Note { get; set; }
	}
}
