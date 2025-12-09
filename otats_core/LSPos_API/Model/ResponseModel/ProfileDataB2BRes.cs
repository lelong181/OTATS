using BusinessLayer.Model.API;
using System;
//using Model.Master;

namespace Model.ResponseModel{

public class ProfileDataB2BRes
{
	public Guid ID { get; set; }

	public string ProfileId { get; set; }

	public Guid? MemberId { get; set; }

	public string ProfileCode { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	public string AlternativeEmail { get; set; }

	public string Phone { get; set; }

	public string Address { get; set; }

	public string PhoneNumber { get; set; }

	public string IdentityCard { get; set; }

	public static ProfileDataB2BRes Parse(Profile profile)
	{
		return (profile == null) ? null : new ProfileDataB2BRes
		{
			Name = ((profile.Name == null) ? "" : profile.Name.ToString()),
			Email = ((profile.Email == null) ? "" : profile.Email.ToString()),
			//AlternativeEmail = ((profile.AlternativeEmail == null) ? "" : profile.AlternativeEmail.ToString()),
			Phone = ((profile.PhoneNumber == null) ? "" : profile.PhoneNumber),
			Address = ((profile.Address == null) ? "" : profile.Address),
			ProfileCode = profile.ProfileCode,
			ProfileId = profile.ID.ToString(),
			IdentityCard = profile.IdentityCard,
			ID = profile.ID
		};
	}

	//private static string ParseAddress(string address, Ward ward)
	//{
	//	District district = ward.District;
	//	City city = district.City;
	//	Region region = city.Region;
	//	return "address";
	//}
}
}