using System;

namespace BusinessLayer.Model.Reports
{
	public class RptRevenueByServicePostingJournal
	{
		public DateTime InvoiceDate { get; set; }

		public string InvoiceCode { get; set; }

		public string AccountCode { get; set; }

		public string GroupCode { get; set; }

		public string GroupName { get; set; }

		public string SubGroupCode { get; set; }

		public string SubGroupName { get; set; }

		public string ServiceCode { get; set; }

		public string ServiceName { get; set; }

		public decimal Amount { get; set; }

		public string CashierShift { get; set; }

		public string BookingCode { get; set; }

		public string CustomerName { get; set; }
	}
}
