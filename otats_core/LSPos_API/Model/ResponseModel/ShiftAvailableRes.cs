using System;

namespace Model.ResponseModel{

public class ShiftAvailableRes
{
	public Guid ShiftID { get; set; }

	public string ShiftName { get; set; }

	public string ShiftDescription { get; set; }

	public string ShiftTypeName { get; set; }

	public DateTime? CheckInDate { get; set; }

	public DateTime? FromDate { get; set; }

	public DateTime? ToDate { get; set; }

	public Guid? ShiftTypeID { get; set; }
}
}