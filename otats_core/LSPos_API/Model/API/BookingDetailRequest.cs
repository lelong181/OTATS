using System;

namespace BusinessLayer.Model.API
{
	public class BookingDetailRequest
	{
		public string SiteCode { get; set; }

		public string ServiceCode { get; set; }

		public DateTime FromSaleDate { get; set; }

		public DateTime ToSaleDate { get; set; }

		public string BookingCode { get; set; }

		public string GroupNo { get; set; }

		public string Channel { get; set; }
	}
}
