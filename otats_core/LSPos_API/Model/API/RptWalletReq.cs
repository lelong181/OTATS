using System;

namespace BusinessLayer.Model.API
{
	public class RptWalletReq
	{
		public string SiteCode { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string WalletCode { get; set; }

		public string BookingCode { get; set; }
	}
}
