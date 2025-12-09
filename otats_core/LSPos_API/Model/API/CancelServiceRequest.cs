using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class CancelServiceRequest
	{
		public string BookingCode { get; set; }

		public string Note { get; set; }

		public List<BookingDetailReq> listRepayService { get; set; }

		public CancelServiceRequest()
		{
			listRepayService = new List<BookingDetailReq>();
		}
	}
}
