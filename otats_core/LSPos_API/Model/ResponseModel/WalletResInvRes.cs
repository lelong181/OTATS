using System.Collections.Generic;

namespace Model.ResponseModel{

public class WalletResInvRes
{
	public string Status { get; set; }

	public string MessageText { get; set; }

	public List<Data> Data { get; set; }
}
}