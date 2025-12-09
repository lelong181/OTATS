using System;

namespace Model.ResponseModel{

public class SiteImageResponse
{
	public Guid? SiteID { get; set; }

	public string SiteCode { get; set; }

	public string Classify { get; set; }

	public string ImageName { get; set; }

	public byte[] Image { get; set; }

	public string ImageType { get; set; }

	public string Descriptions { get; set; }
}
}