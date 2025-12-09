using System;

namespace BusinessLayer.Model.API
{
	public class SessionResponse
	{
		public Guid SessionId { get; set; }

		public int SessionNo { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime? EndTime { get; set; }

		public string Status { get; set; }

		public bool IsMaster { get; set; }

		public string UserName { get; set; }

		public string ComputerName { get; set; }
	}
}
