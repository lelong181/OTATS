using System;

namespace BusinessLayer.Model.API
{
	public class RptLoginLogsReq
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string Username { get; set; }
	}
}
