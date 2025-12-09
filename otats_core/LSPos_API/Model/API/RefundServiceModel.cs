using System;

namespace BusinessLayer.Model.API
{
	public class RefundServiceModel
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string Cashiers { get; set; }
	}
}
