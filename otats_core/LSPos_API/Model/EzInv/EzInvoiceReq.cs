using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.EzInv
{
	public class EzInvoiceReq
	{
		public Guid? CompanyId { get; set; }

		public EzInvoiceTransactionReq Invoice { get; set; }

		public List<EzInvoiceTransactionDetailReq> InvoiceDetails { get; set; }

		public double CmcInvoiceId { get; set; }

		public Guid? InvoiceId { get; set; }

		public Guid SiteID { get; set; }

		public EzInvoiceReq()
		{
			CmcInvoiceId = 0.0;
			Invoice = new EzInvoiceTransactionReq();
			InvoiceDetails = new List<EzInvoiceTransactionDetailReq>();
		}
	}
}
