using System;

namespace Model.ResponseModel.HotelInterface
{

	public class BalanceWalletRes
{
	public string MasterCode { get; set; }

	public decimal Balance { get; set; }

	public DateTime IssuedDate { get; set; }

	public DateTime ExpirationDate { get; set; }

	public bool InActive { get; set; }
	}
}