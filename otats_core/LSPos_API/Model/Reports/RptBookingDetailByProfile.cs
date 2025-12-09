using System;

namespace BusinessLayer.Model.Reports
{
	public class RptBookingDetailByProfile
	{
		public string ProfileCode { get; set; }

		public string ProfileName { get; set; }

		public int Week { get; set; }

		public DateTime CheckinDate { get; set; }

		public string OrderCode { get; set; }

		public string BookingCode { get; set; }

		public string BookingStatus { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public decimal TotalAmount { get; set; }

		public int TotalQuantity { get; set; }

		public int TotalQuantityService { get; set; }

		public int TotalDiscount { get; set; }

		public int TotalDiscountPercent { get; set; }

		public string CustomerName { get; set; }

		public string CustomerEmail { get; set; }

		public string CustomerPhonenumber { get; set; }
	}
}
