using System;

namespace BusinessLayer.Model.Reports
{
	public class RptRevenueByServiceDetail
	{
		public DateTime InvoiceDate { get; set; }

		public string InvoiceCode { get; set; }

		public string ServiceCode { get; set; }

		public string ServiceName { get; set; }

		public decimal Amount { get; set; }

		public string CashierShift { get; set; }

		public string BookingCode { get; set; }

		public string CustomerName { get; set; }
	}
}
