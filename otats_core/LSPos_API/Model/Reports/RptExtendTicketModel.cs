using System;

namespace BusinessLayer.Model.Reports
{
	public class RptExtendTicketModel
	{
		public string AccountCode { get; set; }

		public string AccountName { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }

		public string UserName { get; set; }

		public DateTime ActivityDate { get; set; }

		public DateTime ExtendUsingDate { get; set; }

		public string SiteCode { get; set; }

		public string BookingCode { get; set; }

		public string OrderCode { get; set; }

		public string InvoiceCode { get; set; }

		public string OriginInvoiceCode { get; set; }

		public string ProfileCode { get; set; }

		public string ProfileName { get; set; }

		public DateTime? CheckinDate { get; set; }
	}
}
