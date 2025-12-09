using System;
using System.Collections.Generic;

namespace Model.ResponseModel{

public class DetailRepayServiceRateRes
{
	public Guid BookingID { get; set; }

	public string BookingCode { get; set; }

	public Guid? InvoiceID { get; set; }

	public string InvoiceCode { get; set; }

	public List<ServiceRateRepay> listServiceRate { get; set; }

	public List<AccountRepay> listAccount { get; set; }

	public DetailRepayServiceRateRes()
	{
		listServiceRate = new List<ServiceRateRepay>();
		listAccount = new List<AccountRepay>();
	}
}
}