using System;

namespace BusinessLayer.Model.API
{
	public class RptAcmCountByDateRes
	{
		public DateTime? DateCount { get; set; }

		public string Name { get; set; }

		public string ZoneName { get; set; }

		public int TotalCount { get; set; }
	}
}
