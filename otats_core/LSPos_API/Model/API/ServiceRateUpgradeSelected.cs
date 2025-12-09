using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class ServiceRateUpgradeSelected
	{
		public Guid ID { get; set; }

		public Guid? InvoiceID { get; set; }

		public string InvoiceCode { get; set; }

		public Guid ServiceRateID { get; set; }

		public decimal? Price { get; set; }

		public int Quantity { get; set; }

		public decimal Amount { get; set; }

		public string ServiceRateName { get; set; }

		public string PromotionName { get; set; }

		public Guid? PromotionID { get; set; }

		public List<AccountRepaySelected> ListAccount { get; set; }

		public List<SellCardInput> ListSellCard { get; set; }

		public Guid UpgradeServiceRateID { get; set; }

		public string UpgradeServiceRateName { get; set; }

		public decimal UpgradeServiceRatePrice { get; set; } = default(decimal);


		public decimal UpgradeAmountDifference { get; set; } = default(decimal);


		public ServiceRateUpgradeSelected()
		{
			ListAccount = new List<AccountRepaySelected>();
			ID = Guid.NewGuid();
			ListSellCard = new List<SellCardInput>();
		}
	}
}
