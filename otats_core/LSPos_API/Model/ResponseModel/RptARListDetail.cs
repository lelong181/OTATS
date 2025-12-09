using System;

namespace Model.ResponseModel{

public class RptARListDetail
{
	public string Profile { get; set; }

	public DateTime? TransactionDate { get; set; }

	public decimal? TotalAmount { get; set; }

	public decimal? AllocatedAmount { get; set; }

	public decimal? ReminingAmount { get; set; }

	public string Description { get; set; }

	public string ServiceName { get; set; }

	public Guid? AccountReceivableTransID { get; set; }

	public string BookingCode { get; set; }

	public string StatusAR { get; set; }
}
}