using System;

namespace BusinessLayer.Model.API
{
	public class ChangeTicketRes
	{
		public Guid AccountID { get; set; }

		public string Note { get; set; }

		public string CreatedBy { get; set; }

		public string Type { get; set; }

		public string NewCode { get; set; }

		public string OldCode { get; set; }

		public string ComputerID { get; set; }

		public string Status { get; set; }
	}
}
