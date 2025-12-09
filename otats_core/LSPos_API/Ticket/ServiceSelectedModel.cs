using System;
using System.Collections.Generic;
using BusinessLayer.Model;

namespace Ticket
{
	public class ServiceSelectedModel
	{
		public string ServiceRateID { get; set; }

		public string Title { get; set; }

		public long Amount { get; set; }

		public int Quantity { get; set; }

		public int? TotalQuantity { get; set; }

		public decimal Discount { get; set; }

		public string PromotionID { get; set; }

		public string PromotionLinkID { get; set; }

		public bool HasPromotion { get; set; } = false;


		public decimal SellPrice { get; set; }

		public bool? ManuallyAdjustPrice { get; set; } = false;


		public List<string> AccountCodes { get; set; }

		public List<SellCardInput> ListSellCard { get; set; }

		public Guid? _ShiftID { get; set; }

		public Guid? _ZoneID { get; set; }

		public Guid? _ZoneGroupID { get; set; }

		public string ZoneGroupName { get; set; }

		public string ZoneName { get; set; }

		public List<LockSeat> listSeatLock { get; set; }

		public ServiceSelectedModel()
		{
			Discount = 0m;
			AccountCodes = new List<string>();
			ListSellCard = new List<SellCardInput>();
			listSeatLock = new List<LockSeat>();
		}
	}
}
