using System.Collections.Generic;

namespace Model.ResponseModel{

public class ShiftSellRes
{
	public List<ShiftAvailableRes> ListShift { get; set; }

	public List<ZoneAvailableRes> ListZone { get; set; }

	public List<ZoneGroupAvailableRes> ListZoneGroup { get; set; }

	public ShiftSellRes()
	{
		ListShift = new List<ShiftAvailableRes>();
		ListZone = new List<ZoneAvailableRes>();
		ListZoneGroup = new List<ZoneGroupAvailableRes>();
	}
}
}