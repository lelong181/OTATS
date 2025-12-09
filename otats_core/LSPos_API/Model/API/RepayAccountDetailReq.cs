using System;

namespace BusinessLayer.Model.API
{
	public class RepayAccountDetailReq
	{
		public Guid AccountID { get; set; }

		public string AccountCode { get; set; }

		public Guid? ServiceID { get; set; }

		public string CardID { get; set; }

		public Guid? BookingDetailSeatID { get; set; }
	}
}
