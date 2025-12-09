using System;

namespace BusinessLayer.Model.API
{
	public class UgradeAccountDetailReq
	{
		public Guid AccountID { get; set; }

		public string AccountCode { get; set; }

		public string CardID { get; set; }
	}
}
