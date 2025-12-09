using System;

namespace BusinessLayer.Model.API
{
	public class InvoiceByDateRequest
	{
		public DateTime InvoiceDate { get; set; }

		public string InvoiceCode { get; set; }
	}
}
