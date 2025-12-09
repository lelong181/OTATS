using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.Wallet
{
	public class CheckoutWalletReq
	{
		public Guid? BookingID { get; set; }

		public string BookingCode { get; set; }

		public string WalletCode { get; set; }

		public Guid? AccountID { get; set; }

		public Guid? AccountDetailID { get; set; }

		public string Description { get; set; }

		public Guid ProfileId { get; set; }

		public Guid SiteId { get; set; }

		public Guid? MemberId { get; set; }

		public Guid? SessionID { get; set; }

		public string SiteCode { get; set; }

		public string CreatedBy { get; set; }

		public string ComputerId { get; set; }

		public string ZoneId { get; set; }

		public string Channel { get; set; }

		public Guid? ServiceRateWalletCheckoutID { get; set; }

		public List<CustomerBookingWalletReq> BookingCustomers { get; set; }

		public List<PaymentRechargeMoneyReq> listPayment { get; set; }

		public CheckoutWalletReq()
		{
			BookingCustomers = new List<CustomerBookingWalletReq>();
			listPayment = new List<PaymentRechargeMoneyReq>();
		}
	}
}
