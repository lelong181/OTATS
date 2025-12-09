using System;

namespace BusinessLayer.Model.Reports
{
	public class RptBookingStatus
	{
		public string BookingCode { get; set; }

		public string ProfileCode { get; set; }

		public string ProfileName { get; set; }

		public int QtyUsed { get; set; }

		public int QtyExpiration { get; set; }

		public int QtyNotUse { get; set; }

		public int QtyUse { get; set; }

		public int QtyCancel { get; set; }

		public string BookingStatus { get; set; }

		public DateTime CheckinDate { get; set; }

		public DateTime SaleDate { get; set; }

		public string ServiceCode { get; set; }

		public string ServiceName { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Amount { get; set; }
	}
}
