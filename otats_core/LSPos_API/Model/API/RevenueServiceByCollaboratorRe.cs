using System;

namespace BusinessLayer.Model.API
{
	public class RevenueServiceByCollaboratorRequest
	{
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string SiteCode { get; set; }

		public string CollaboratorID { get; set; }
	}
}
