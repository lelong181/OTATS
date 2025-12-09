using System;

namespace BusinessLayer.Model.API
{
	public class LoginLogsRequest
	{
		public string UserName { get; set; }

		public Guid ComputerID { get; set; }

		public string IP { get; set; }
	}
}
