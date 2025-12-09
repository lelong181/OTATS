using System;

namespace BusinessLayer.Model.API
{
	public class ServiceRateResponse
	{
		public string ServicePackageID { get; set; }

		public string ServiceID { get; set; }

		public string ServiceName { get; set; }

		public string ServiceSubGroupName { get; set; }

		public int Quantity { get; set; }

		public decimal Amount { get; set; }

		public decimal Price { get; set; }

		public int OriginQuantity { get; set; }

		public string AccountCodes { get; set; }

		public string CardIds { get; set; }

		public DateTime? SaleDate { get; set; }

		public DateTime? UsingDate { get; set; }

		public DateTime? ExpirationDate { get; set; }

		public string Status { get; set; }

		public string StatusStr { get; set; }

		public int PrintCount { get; set; }

		public bool HasUsing { get; set; }

		public bool? IsMasterCode { get; set; }

		public string Classify { get; set; }

		public int Sequence { get; set; }

		public byte[] Image { get; set; }

		public string AccountID { get; set; }

		public string InvoiceID { get; set; }
	}
}
