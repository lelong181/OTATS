using System;

namespace BusinessLayer.Model.API
{
	public class WalletRepayRes
	{
		public string BookingCode { get; set; }

		public string WalletCode { get; set; }

		public DateTime CheckinDate { get; set; }

		public Guid BookingID { get; set; }

		public Guid WalletID { get; set; }
	}
}
