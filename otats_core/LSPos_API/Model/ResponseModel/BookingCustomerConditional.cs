using System;

namespace Model.ResponseModel{

public class BookingCustomerConditional
{
	public Guid ID { get; set; }

	public Guid SiteID { get; set; }

	public Guid? ShiftID { get; set; }

	public string ConditionalKey { get; set; }

	public decimal ConditionalValue { get; set; }

	public bool? Inactive { get; set; }

	public bool? IsDelete { get; set; }

	public int CurrentTotalQuantity { get; set; }
}
}