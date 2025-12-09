namespace BusinessLayer.Model.API
{
	public class ProfileListReq
	{
		public int PageSize { get; set; }

		public int CurrentPage { get; set; }

		public string Keyword { get; set; }
	}
}
