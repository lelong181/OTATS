using System;

namespace Ticket
{
	public class SessionModel : BaseModel
	{
		public int ID { get; set; }

		public int ComputerID { get; set; }

		public int UserID { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public int Status { get; set; }

		public int SessionNo { get; set; }
	}
}
