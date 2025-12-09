using System;

namespace BusinessLayer.Model.Wallet
{
	public class PaymentRechargeMoneyReq
	{
		public Guid PaymentTypeID { get; set; }

		public string PaymentTypeName { get; set; }

		public long Amount { get; set; }

		public string SessionID { get; set; }
	}
}
