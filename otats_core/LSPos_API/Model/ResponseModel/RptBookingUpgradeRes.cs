using System;

namespace Model.ResponseModel{

public class RptBookingUpgradeRes
{
	public Guid? BookingUpgradeID { get; set; }

	public string Note { get; set; }

	public string BookingCode { get; set; }

	public string InvoiceCode { get; set; }

	public string Cashier { get; set; }

	public int? SessionNo { get; set; }

	public decimal? UpgradeAmountDifference { get; set; }

	public decimal? UpgradeAmount { get; set; }

	public string UpgradeServiceRateName { get; set; }

	public decimal? OriginAmount { get; set; }

	public string OriginServiceRateName { get; set; }

	public DateTime? UpgradeTime { get; set; }

	public string ComputerName { get; set; }

	public string InvoiceCreated { get; set; }
}
}