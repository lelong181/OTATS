namespace BusinessLayer.Model.API
{
	public class KiosCheckinRequest
	{
		public string SearchBy { get; set; }

		public string Keyword { get; set; }

		public string SiteCode { get; set; }

		public string LangCode { get; set; }
	}
}
