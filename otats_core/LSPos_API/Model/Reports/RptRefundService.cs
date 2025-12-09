using System;

namespace BusinessLayer.Model.Reports
{
	public class RptRefundService
	{
		public long SerialNumber { get; set; }

		public DateTime SaleDate { get; set; }

		public string Cashier { get; set; }

		public string CashierRepay { get; set; }

		public string ComputerName { get; set; }

		public string ComputerNameRepay { get; set; }

		public string ServiceCode { get; set; }

		public string ServiceName { get; set; }

		public string ServiceSubGroupName { get; set; }

		public decimal TotalMoney { get; set; }

		public string Description { get; set; }

		public string InvoiceCode { get; set; }

		public string OriginInvoiceCode { get; set; }

		public string ProfileCode { get; set; }

		public string ProfileName { get; set; }

		public DateTime CheckinDate { get; set; }

		public string OrderCode { get; set; }

		public string DifferentDate { get; set; }

		public DateTime OriginInvoiceDate { get; set; }

		public DateTime InvoiceDate { get; set; }
	}
}
