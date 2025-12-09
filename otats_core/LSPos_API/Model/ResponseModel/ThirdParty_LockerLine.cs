using System;

namespace Model.ResponseModel{

public class ThirdParty_LockerLine
{
	public Guid ID { get; set; }

	public int ZoneID { get; set; }

	public int LineID { get; set; }

	public string LineName { get; set; }

	public int Priority { get; set; }

	public int? PercentUse { get; set; }

	public string TypeUsed { get; set; }

	public bool? Inactive { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }

	public string UpdatedBy { get; set; }

	public DateTime? UpdatedDate { get; set; }

	public string FlexCol1 { get; set; }

	public string FlexCol2 { get; set; }

	public string FlexCol3 { get; set; }

	public string FlexCol4 { get; set; }

	public string FlexCol5 { get; set; }
}
}