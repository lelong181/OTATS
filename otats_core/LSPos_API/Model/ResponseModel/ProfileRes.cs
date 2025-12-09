using System;

namespace Model.ResponseModel{

public class ProfileRes
{
	public string ProfileId { get; set; }

	public string IdentityCard { get; set; }

	public string Email { get; set; }

	public string ProfileCode { get; set; }

	public string Name { get; set; }

	public string Gender { get; set; }

	public DateTime? BirthDate { get; set; }

	public string AlternativeEmail { get; set; }

	public string CellPhone { get; set; }

	public string PhoneNumber { get; set; }

	public string Address { get; set; }

	public Guid? WardID { get; set; }

	public string PostalCode { get; set; }

	public int? CardIssueCount { get; set; }

	public Guid? ProfileLevelID { get; set; }

	public string ProfileType { get; set; }

	public Guid? SaleInChargeID { get; set; }

	public bool Inactive { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }

	public string UpdatedBy { get; set; }

	public DateTime? UpdatedDate { get; set; }

	public Guid? MemberID { get; set; }
}
}