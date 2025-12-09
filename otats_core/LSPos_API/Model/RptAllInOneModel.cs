using System;

namespace BusinessLayer.Model
{
	public class RptAllInOneModel
	{
		public DateTime? CheckinDate { get; set; }

		public DateTime? ACMDate { get; set; }

		public string AccountCode { get; set; }

		public string ACM { get; set; }

		public string CardType { get; set; }

		public string ServiceSubGroupName { get; set; }

		public string Cashier { get; set; }

		public string InvoiceCode { get; set; }

		public string ServiceRateName { get; set; }

		public string BookingCode { get; set; }

		public int? SessionNo { get; set; }

		public decimal? TotalMoney { get; set; }

		public string ZoneCode { get; set; }

		public string TAName { get; set; }

		public string COLName { get; set; }

		public int? PrintCount { get; set; }

		public string Nationality { get; set; }

		public string HDV { get; set; }

		public string OrderCode { get; set; }

		public string Notes { get; set; }

		public string AccountStatus { get; set; }

		public bool HasUsing { get; set; }
	}
}
