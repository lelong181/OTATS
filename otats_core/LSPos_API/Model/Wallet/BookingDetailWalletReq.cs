using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.Wallet
{
	public class BookingDetailWalletReq
	{
		public string Id { get; set; }

		public int Quantity { get; set; }

		public string Checkin { get; set; }

		public List<string> AccountCodes { get; set; }

		public decimal Discount { get; set; }

		public string PromotionID { get; set; }

		public string PromotionLinkID { get; set; }

		public BookingDetailWalletReq()
		{
			AccountCodes = new List<string>();
		}

		public Guid GetRateId()
		{
			return Guid.Parse(Id);
		}

		public DateTime GetCheckIn()
		{
			return DateTime.Parse(Checkin);
		}
	}
}
