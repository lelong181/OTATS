using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.Wallet
{
	public class WalletInforRes
	{
		public string WalletCode { get; set; }

		public string CardIDOrigin { get; set; }

		public string Description { get; set; }

		public Guid? AccountID { get; set; }

		public Guid? AccountDetailID { get; set; }

		public Guid? BookingID { get; set; }

		public string BookingStatus { get; set; }

		public string Status { get; set; }

		public string BookingCode { get; set; }

		public decimal Deposit { get; set; }

		public decimal CreditLimit { get; set; }

		public decimal TotalMoney { get; set; }

		public string MemberID { get; set; }

		public string MemberFullName { get; set; }

		public DateTime? CheckInDate { get; set; }

		public List<WalletPayment> ListPayments { get; set; }

		public List<WalletService> ListService { get; set; }

		public List<WalletServiceRateRes> ListServiceRateRes { get; set; }

		public BookingWalletInfo BookingWalletInfor { get; set; }

		public WalletInforRes()
		{
			ListPayments = new List<WalletPayment>();
			ListService = new List<WalletService>();
			ListServiceRateRes = new List<WalletServiceRateRes>();
			BookingWalletInfor = null;
		}
	}
}
