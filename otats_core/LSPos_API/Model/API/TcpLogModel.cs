using System;

namespace BusinessLayer.Model.API
{
	public class TcpLogModel
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string DeviceId { get; set; }

		public string ServiceCode { get; set; }
	}
}
