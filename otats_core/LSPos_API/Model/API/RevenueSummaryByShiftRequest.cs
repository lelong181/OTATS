using BusinessLayer.Model.Reports;

namespace BusinessLayer.Model.API
{
	public class RevenueSummaryByShiftRequest : CommonParamReport
	{
		public string Cashier { get; set; }

		public string Channels { get; set; }

		public int TypeView { get; set; }
	}
}
