namespace BusinessLayer.Model.Reports
{
	public class RptRevenueSummaryByShift
	{
		public int SerialNumber { get; set; }

		public string ID { get; set; }

		public string PaymentType { get; set; }

		public decimal TotalMoney { get; set; }

		public decimal TotalMoneyBeforeTax { get; set; }
	}
}
