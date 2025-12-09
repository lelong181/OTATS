using System;

namespace BusinessLayer.Model.API
{
	public class RepayPaymentTypeReq
	{
		public Guid PaymentID { get; set; }

		public string Name { get; set; }

		public decimal Amount { get; set; }
	}
}
