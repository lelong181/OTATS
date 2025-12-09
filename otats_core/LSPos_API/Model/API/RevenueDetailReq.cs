using BusinessLayer.Model.Reports;

namespace BusinessLayer.Model.API
{
	public class RevenueDetailReq : CommonParamReport
	{
		public string ServiceRateId { get; set; }

		public string ServiceGroupId { get; set; }

		public string ServiceSubGroupId { get; set; }

		public string Sale { get; set; }

		public string Channels { get; set; }

		public string Profile { get; set; }

		public bool NotFOC { get; set; } = false;


		public string ShiftID { get; set; }
	}
}
