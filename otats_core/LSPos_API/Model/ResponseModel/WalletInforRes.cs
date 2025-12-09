using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Model.ResponseModel{

public class WalletInforRes
{
	public string WalletCode { get; set; }

	public string Description { get; set; }

	public Guid? AccountID { get; set; }

	public Guid? AccountDetailID { get; set; }

	public Guid? BookingID { get; set; }

	public string BookingCode { get; set; }

	public string BookingStatus { get; set; }

	public decimal Deposit { get; set; }

	public decimal CreditLimit { get; set; }

	public decimal TotalMoney { get; set; }

	public string Status { get; set; }

	public Guid? MemberID { get; set; }

	public string MemberFullName { get; set; }

	public DateTime CheckinDate { get; set; }

	public List<WalletPayment> ListPayments { get; set; }

	[JsonIgnore]
	public List<WalletServiceRate> ListServiceRate { get; set; }

	public List<WalletServiceRateRes> ListServiceRateRes { get; set; }

	public BookingWalletInfo BookingWalletInfor { get; set; }

	public WalletInforRes()
	{
		ListPayments = new List<WalletPayment>();
		ListServiceRate = new List<WalletServiceRate>();
		ListServiceRateRes = new List<WalletServiceRateRes>();
		BookingWalletInfor = null;
	}
}
}