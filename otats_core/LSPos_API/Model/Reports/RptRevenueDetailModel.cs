namespace BusinessLayer.Model.Reports
{
	public class RptRevenueDetailModel
	{
		public string ServiceRateName { get; set; }

		public int Quantity { get; set; }

		public double PriceNet { get; set; }

		public double PriceDiscount { get; set; }

		public double AmountDiscount { get; set; }

		public double DiscountRate { get; set; }

		public decimal AmountGross { get; set; }

		public decimal AmountSVC { get; set; }

		public decimal AmountTTTDB { get; set; }

		public decimal AmountVAT { get; set; }

		public decimal TotalAmount { get; set; }
	}
}
