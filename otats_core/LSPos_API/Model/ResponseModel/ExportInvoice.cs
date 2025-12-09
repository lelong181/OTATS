using System;

namespace Model.ResponseModel{

public class ExportInvoice : ReportCMC
{
	public Guid? BookingDetailID { get; set; }

	public Guid? ShiftID { get; set; }
}
}