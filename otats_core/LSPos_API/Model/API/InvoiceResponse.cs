using System;

namespace BusinessLayer.Model.API
{
	public class InvoiceResponse
	{
		public Guid ID { get; set; }

		public string InvoiceCode { get; set; }

		public string ProfileID { get; set; }

		public DateTime UsingDate { get; set; }

		public DateTime InvoiceDate { get; set; }

		public decimal Amount { get; set; }

		public decimal? TotalMoney { get; set; }

		public int SessionNo { get; set; }

		public string Description { get; set; }

		public string InvoicePaymentType { get; set; }

		public string Cashier { get; set; }

		public int PrintCount { get; set; }

		public int PrintTaxCount { get; set; }

		public string Channel { get; set; }
	}
}
