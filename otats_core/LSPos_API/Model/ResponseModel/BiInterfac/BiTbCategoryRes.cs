using System;

namespace Model.ResponseModel.BiInterface{

public class BiTbCategoryRes
{
	public Guid CategoryId { get; set; }

	public int EntityType { get; set; }

	public Guid? ParentCategoryId { get; set; }

	public string CategoryCode { get; set; }

	public string CategoryName { get; set; }

	public string RecursiveName { get; set; }

	public string CategoryNameExt { get; set; }

	public bool? ShowNameExt { get; set; }

	public bool? InheritMask { get; set; }

	public bool? InheritLocation { get; set; }
}
}