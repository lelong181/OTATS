namespace BusinessLayer.Model.Reports
{
	public class RptServiceSales
	{
		public string ServiceRateName { get; set; }

		public long Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Amount { get; set; }

		public decimal TotalTax { get; set; }

		public decimal TotalMoney { get; set; }

		public decimal TotalDiscount { get; set; }
	}
}
