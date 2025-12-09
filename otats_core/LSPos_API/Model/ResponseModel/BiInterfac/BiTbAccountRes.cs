using System;

namespace Model.ResponseModel.BiInterface{

public class BiTbAccountRes
{
	public Guid AccountId { get; set; }

	public int EntityType { get; set; }

	public int AccountStatus { get; set; }

	public string AccountCode { get; set; }

	public string DisplayName { get; set; }

	public string AccountCodeExt { get; set; }

	public Guid? CategoryId { get; set; }

	public Guid? ParentAccountId { get; set; }

	public string CustomerType { get; set; }
}
}