using System;

namespace BusinessLayer.Model
{
	public class ExportInvoice : ReportCMC
	{
		public Guid? BookingDetailID { get; set; }

		public Guid? ShiftID { get; set; }

		public string HasEzInvoice { get; set; }

		public string lstCmCInvoice { get; set; }
	}
}
