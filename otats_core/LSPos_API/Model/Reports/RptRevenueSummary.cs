namespace BusinessLayer.Model.Reports
{
	public class RptRevenueSummary
	{
		public string ServiceSubGroupName { get; set; }

		public string ServiceRateName { get; set; }

		public decimal TotalMoneyBeforeTax { get; set; }

		public decimal TotalTax { get; set; }

		public decimal Price { get; set; }

		public int Quantity { get; set; }

		public decimal TotalMoney { get; set; }

		public string Status { get; set; }
	}
}
