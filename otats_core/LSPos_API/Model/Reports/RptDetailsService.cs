using System;

namespace BusinessLayer.Model.Reports
{
	public class RptDetailsService
	{
		public string InvoiceCode { get; set; }

		public string ServiceCode { get; set; }

		public DateTime SaleDate { get; set; }

		public DateTime IssuedDate { get; set; }

		public DateTime ExpirationDate { get; set; }

		public string ServiceName { get; set; }

		public string ServiceGroup { get; set; }

		public long Quantity { get; set; }

		public decimal Price { get; set; }

		public string Status { get; set; }

		public string StatusStr { get; set; }

		public decimal TotalDiscount { get; set; }

		public string Cashier { get; set; }

		public string ACM_Name { get; set; }

		public DateTime? ACM_UsingDate { get; set; }

		public string Channel { get; set; }

		public string Source { get; set; }

		public string OrderCode { get; set; }

		public string ServiceRateName { get; set; }
	}
}
