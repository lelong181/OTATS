using System;

namespace BusinessLayer.Model.API
{
	public class WalletRepayReq
	{
		public string BookingCode { get; set; }

		public string WalletCode { get; set; }

		public DateTime WalletDate { get; set; }

		public string SiteCode { get; set; }
	}
}
