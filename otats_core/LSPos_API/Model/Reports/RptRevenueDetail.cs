using System;

namespace BusinessLayer.Model.Reports
{
	public class RptRevenueDetail
	{
		public DateTime? SaleDate { get; set; }

		public DateTime? InvoiceDate { get; set; }

		public string InvoiceCode { get; set; }

		public int? Quantity { get; set; }

		public decimal? TotalMoney { get; set; }

		public string GroupCode { get; set; }

		public string GroupName { get; set; }

		public string SubGroupCode { get; set; }

		public string SubGroupName { get; set; }

		public string ServiceCode { get; set; }

		public string ServiceName { get; set; }

		public string GroupBy { get; set; }

		public string TransactionNo { get; set; }

		public int? SessionNo { get; set; }

		public string Cashier { get; set; }

		public string ComputerName { get; set; }

		public string ZoneCode { get; set; }

		public string BookingCode { get; set; }

		public string Description { get; set; }

		public string ProfileCode { get; set; }

		public string ProfileName { get; set; }

		public string SaleInChargeName { get; set; }

		public string SaleInChargeEmail { get; set; }

		public string SaleInChargeTelePhone { get; set; }

		public string SaleInChargeMobiPhone { get; set; }

		public string OrderCode { get; set; }

		public string Channel { get; set; }

		public string Source { get; set; }

		public string ServiceRateName { get; set; }

		public string Status { get; set; }

		public string OriginInvoiceCode { get; set; }

		public string PromotionName { get; set; }

		public string IdentityCard { get; set; }
	}
}
