using System;
using System.Collections.Generic;
using BaseBusiness;

namespace BusinessLayer.Model.Sell
{
	public class ServiceRateModel
	{
		public bool? ManuallyAdjustPrice { get; set; } = false;


		public bool? PrintTicketBy { get; set; }

		public string Color { get; set; }

		public string ServiceID { get; set; }

		public string ServiceRateID { get; set; }

		public List<string> ServiceSubGroupIDs { get; set; }

		public string Title { get; set; }

		public string CardType { get; set; }

		public int? DailyQuantity { get; set; }

		public int? SaleQuantity { get; set; }

		public string QuantityStr { get; set; } = "Color Display";


		public string ColorDisplay { get; set; }

		public Guid? ServiceRateGroupID { get; set; }

		public decimal NetPrice { get; set; }

		public decimal SellPrice { get; set; }

		public long SellPriceDisplay
		{
			get
			{
				if (SellPrice > 0m)
				{
					return Convert.ToInt64(SellPrice);
				}
				if (NetPrice > 0m)
				{
					return Convert.ToInt64(NetPrice);
				}
				return 0L;
			}
			set
			{
			}
		}

		public bool? StopSell { get; set; }

		public bool? StopPromotion { get; set; }

		public List<PromotionModel> Promotions { get; set; }

		public bool HasSeat { get; set; } = false;


		public Guid? ZoneID { get; set; }

		public string ZoneName { get; set; }

		public List<LockSeat> listSeatLock { get; set; }

		public ServiceRateModel()
		{
			ServiceSubGroupIDs = new List<string>();
			Promotions = new List<PromotionModel>();
			listSeatLock = new List<LockSeat>();
		}

        public ServiceRateModel(int Quantity, string Title, string ServiceRateID, decimal SellPrice)
        {
			this.SellPrice = SellPrice;
			this.Title = Title;
			this.ServiceRateID = ServiceRateID;
			this.SellPrice = SellPrice;
			this.SaleQuantity = Quantity;

        }
    }
}
