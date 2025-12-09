using System;

namespace Model.ResponseModel.BiInterface{

public class BiTbProductRes
{
	public Guid ProductId { get; set; }

	public Guid CategoryId { get; set; }

	public int ProductType { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }

	public int ParentEntityType { get; set; }

	public Guid? ParentEntityId { get; set; }

	public int ProductStatus { get; set; }

	public Guid? PrintGroupTagId { get; set; }

	public Guid? AccountCategoryId { get; set; }

	public int GroupTicketOption { get; set; }

	public string ProductNameExt { get; set; }

	public bool? ShowNameExt { get; set; }

	public Guid? FinanceGroupTagId { get; set; }

	public Guid? AdmGroupTagId { get; set; }

	public Guid? AreaGroupTagId { get; set; }

	public Guid? LocationId { get; set; }
}
}