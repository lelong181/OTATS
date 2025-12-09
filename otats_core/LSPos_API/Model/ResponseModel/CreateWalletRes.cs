using System;

namespace Model.ResponseModel{

public class CreateWalletRes
{
	public string WalletCode { get; set; }

	public Guid AccountID { get; set; }

	public Guid AccountDetailID { get; set; }
}
}