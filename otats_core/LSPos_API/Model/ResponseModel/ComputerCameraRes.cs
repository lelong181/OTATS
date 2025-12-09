using System;

namespace Model.ResponseModel{

public class ComputerCameraRes
{
	public Guid? ID { get; set; }

	public string Description { get; set; }

	public string URL { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public bool? Inactive { get; set; }

	public string FlexCol1 { get; set; }

	public string FlexCol2 { get; set; }
}
}