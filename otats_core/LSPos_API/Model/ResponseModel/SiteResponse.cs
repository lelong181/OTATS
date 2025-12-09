using System;

namespace Model.ResponseModel{

public class SiteResponse
{
	public Guid ID { get; set; }

	public string SiteCode { get; set; }

	public string Title { get; set; }

	public string Email { get; set; }

	public string Hotline { get; set; }

	public string PhoneNum { get; set; }

	public string Address { get; set; }

	public string City { get; set; }

	public string Country { get; set; }

	public string Description { get; set; }

	public string Location { get; set; }

	public Guid? B2c_ProfileID { get; set; }

	public string PaymentPolicy { get; set; }

	public string SitePolicy { get; set; }

	public string ServerName { get; set; }

	public string Type { get; set; }

	public string LinkFacebook { get; set; }

	public string LinkInstagram { get; set; }

	public string LinkYoutube { get; set; }

	public string ColorLayout { get; set; }

	public string ColorText { get; set; }

	public string ColorButton { get; set; }

	public string LinkCRM { get; set; }

	public string LinkSMS { get; set; }

	public string TitleWeb { get; set; }

	public string DescriptionWeb { get; set; }

	public string CompanyName { get; set; }
}
}