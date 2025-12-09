using System;

namespace BusinessLayer.Model.Reports
{
	public class RptChangeTicketHistory
	{
		public DateTime UpdateDate { get; set; }

		public string UpdateBy { get; set; }

		public string ComputerName { get; set; }

		public string OldCode { get; set; }

		public string NewCode { get; set; }

		public string Note { get; set; }

		public string Status { get; set; }

		public string ServiceName { get; set; }
	}
}
