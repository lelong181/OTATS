namespace BusinessLayer.Model.API
{
	public class RptCouponRequest
	{
		public string SiteCode { get; set; }

		public string Code { get; set; }

		public string Status { get; set; }

		public int StatusCurrent { get; set; }
	}
}
