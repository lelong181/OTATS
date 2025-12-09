using System;

namespace Model.ResponseModel.BiInterface{

public class BiTbTagRes
{
	public Guid TagId { get; set; }

	public int EntityType { get; set; }

	public string TagCode { get; set; }

	public string TagName { get; set; }
}
}