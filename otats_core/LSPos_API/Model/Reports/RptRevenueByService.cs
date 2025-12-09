namespace BusinessLayer.Model.Reports
{
	public class RptRevenueByService
	{
		public string GroupCode { get; set; }

		public string GroupName { get; set; }

		public string SubGroupCode { get; set; }

		public string SubGroupName { get; set; }

		public string ServiceCode { get; set; }

		public string ServiceName { get; set; }

		public decimal DAY_NET { get; set; }

		public decimal DAY_GROSS { get; set; }

		public decimal MONTH_NET { get; set; }

		public decimal MONTH_GROSS { get; set; }

		public decimal YEAR_NET { get; set; }

		public decimal YEAR_GROSS { get; set; }
	}
}
