using System;

namespace BusinessLayer.Model.API
{
	public class RptLoginLogsRes
	{
		public string UserName { get; set; }

		public string FullName { get; set; }

		public DateTime LoginTime { get; set; }

		public string ComputerName { get; set; }

		public string IP { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }

		public string DepartmentName { get; set; }
	}
}
