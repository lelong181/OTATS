using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class ListServiceRateUpgradeRes
	{
		public Guid? BookingID { get; set; }

		public string BookingCode { get; set; }

		public Guid? InvoiceID { get; set; }

		public string InvoiceCode { get; set; }

		public Guid? ProfileID { get; set; }

		public string Channel { get; set; }

		public List<ServiceRatePrepareUpgrade> ListServiceRate { get; set; }

		public List<AccountPrepareUpgrade> ListAccount { get; set; }

		public ListServiceRateUpgradeRes()
		{
			ListServiceRate = new List<ServiceRatePrepareUpgrade>();
			ListAccount = new List<AccountPrepareUpgrade>();
		}
	}
}
