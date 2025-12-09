using System;

namespace BusinessLayer.Model.Reports
{
	public class RptBookingDetail
	{
		public string BookingCode { get; set; }

		public string GroupNo { get; set; }

		public string BookingStatus { get; set; }

		public DateTime CheckinDate { get; set; }

		public DateTime SaleDate { get; set; }

		public string Channel { get; set; }

		public string OrderCode { get; set; }

		public string ProfileCode { get; set; }

		public string ProfileName { get; set; }

		public string ProfileLevel { get; set; }

		public string SaleInCharge { get; set; }

		public string GroupCode { get; set; }

		public string GroupName { get; set; }

		public string SubGroupCode { get; set; }

		public string SubGroupName { get; set; }

		public string ServiceCode { get; set; }

		public string ServiceName { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Amount { get; set; }

		public string ProfileType { get; set; }

		public string Address { get; set; }

		public string CityName { get; set; }

		public string RegionName { get; set; }

		public string GuideName { get; set; }

		public string LeaderName { get; set; }

		public string OperatorName { get; set; }
	}
}
