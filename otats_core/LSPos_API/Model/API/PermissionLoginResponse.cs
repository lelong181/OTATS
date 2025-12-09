using System;

namespace BusinessLayer.Model.API
{
	public class PermissionLoginResponse
	{
		public string Username { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }

		public string Description { get; set; }

		public string POS_ProfileId { get; set; }

		public Guid SiteID { get; set; }

		public string SiteCode { get; set; }

		public string SiteI18n_Name { get; set; }

		public string SiteI18n_Description { get; set; }

		public string TypeTicketQR { get; set; } = "HEXA";


		public string SiteType { get; set; } = "NORMAL";


		public string Flex1 { get; set; }

		public m_ApiConnect ApiConnect { get; set; }
	}
}
