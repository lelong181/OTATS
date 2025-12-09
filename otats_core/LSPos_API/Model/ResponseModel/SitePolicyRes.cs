using System;

namespace Model.ResponseModel{

public class SitePolicyRes
{
	public Guid SiteID { get; set; }

	public string SiteCode { get; set; }

	public string SitePolicy { get; set; }

	public string PaymentPolicy { get; set; }
}
}