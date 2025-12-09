using System;

namespace BusinessLayer.Model.Wallet
{
	public class WalletServiceRateRes
	{
		public Guid? ServiceRateID { get; set; }

		public string ServiceRateName { get; set; }

		public DateTime? SaleDate { get; set; }

		public decimal? Price { get; set; }

		public int? Quantity { get; set; }

		public decimal Discount { get; set; }

		public decimal TotalAmount { get; set; }

		public string Status { get; set; }

		public string Description { get; set; }
	}
}
