using System;

namespace Model.ResponseModel{

public class ZoneAvailableRes
{
	public Guid ShiftID { get; set; }

	public Guid ZoneID { get; set; }

	public string ZoneName { get; set; }

	public string ZoneCode { get; set; }

	public string ZoneService { get; set; }
}
}