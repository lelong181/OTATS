using System;

namespace BusinessLayer.Model.API
{
	public class BookingRateResponse
	{
		public Guid ServiceRateID { get; set; }

		public DateTime SaleDate { get; set; }

		public Guid BookingDetailID { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Amount { get; set; }

		public Guid? PromotionID { get; set; }

		public string ServiceRateName { get; set; }

		public bool IsSplit { get; set; }

		public int RowState { get; set; }

		public Guid? PromotionLinkID { get; set; }

		public string GroupCode { get; set; }

		public bool? PrintTicketBy { get; set; }
	}
}
