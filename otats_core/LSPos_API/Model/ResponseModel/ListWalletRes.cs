using System;

namespace Model.ResponseModel{

public class ListWalletRes
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

	public DateTime? TransactionDate { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }

	public string MemberFullName { get; set; }

	public string MemberPhoneNumber { get; set; }

	public string MemberIdentityCard { get; set; }

	public string MemberEmail { get; set; }

	public string LockerCode { get; set; }
}
}