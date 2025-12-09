using System;

namespace BusinessLayer.Model.Reports
{
	public class RevenueServiceByCollaborator
	{
		public string CollaboratorCode { get; set; }

		public string CollaboratorName { get; set; }

		public string MobilePhone { get; set; }

		public string BookingCode { get; set; }

		public string AccountCode { get; set; }

		public string ServiceName { get; set; }

		public string ServiceRateName { get; set; }

		public decimal TotalMoney { get; set; }

		public string Channel { get; set; }

		public string Source { get; set; }

		public string InvoiceCode { get; set; }

		public DateTime InvoiceDate { get; set; }

		public int Quantity { get; set; }
	}
}
