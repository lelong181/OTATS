using System;

namespace Model.ResponseModel{

public class SeatAvailableReq
{
	public string LangCode { get; set; }

	public Guid ShiftID { get; set; }

	public Guid ZoneID { get; set; }

	public Guid? ZoneGroupID { get; set; }

	public Guid ServiceRateID { get; set; }

	public string Type { get; set; }
}
}