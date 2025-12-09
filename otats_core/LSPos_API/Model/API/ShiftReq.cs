using System;

namespace BusinessLayer.Model.API
{
	public class ShiftReq
	{
		public string ShiftID { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }
	}
}
