using System;

namespace Model.ResponseModel{

public class TicketWarehouseRes
{
	public string SiteCode { get; set; }

	public string SiteName { get; set; }

	public string ServiceRateGroupName { get; set; }

	public string ShortName { get; set; }

	public string ServiceRateName { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public string ProfileName { get; set; }

	public string ProfileCode { get; set; }

	public int? Total_Empty { get; set; }

	public int? Total_Other { get; set; }

	public int? Total_Sold { get; set; }
}
}