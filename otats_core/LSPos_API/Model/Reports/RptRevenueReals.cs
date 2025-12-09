using System;

namespace BusinessLayer.Model.Reports
{
	public class RptRevenueReals
	{
		public string InvoiceCode { get; set; }

		public DateTime InvoiceDate { get; set; }

		public DateTime BookingDate { get; set; }

		public string BookingCode { get; set; }

		public DateTime PrintDate { get; set; }

		public string ServiceCode { get; set; }

		public string ServiceName { get; set; }

		public string ServiceRateName { get; set; }

		public int Qty { get; set; }

		public decimal Price { get; set; }

		public decimal TotalMoney { get; set; }

		public string AccountCode { get; set; }

		public DateTime UsingDate { get; set; }

		public DateTime ExpirationDate { get; set; }

		public string ProfileCode { get; set; }

		public string ProfileName { get; set; }

		public string Description { get; set; }

		public string StatusText { get; set; }
	}
}
