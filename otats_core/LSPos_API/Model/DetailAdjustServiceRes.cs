using System;
using System.Collections.Generic;

namespace BusinessLayer.Model
{
	public class DetailAdjustServiceRes
	{
		public Guid BookingID { get; set; }

		public string BookingCode { get; set; }

		public Guid? InvoiceID { get; set; }

		public string InvoiceCode { get; set; }

		public List<ServiceRateAdjust> listService { get; set; }

		public DetailAdjustServiceRes()
		{
			listService = new List<ServiceRateAdjust>();
		}
	}
}
