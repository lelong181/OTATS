using System;

namespace Ticket
{
	public class EmployeeModel : BaseModel
	{
		public int ID { get; set; }

		public string IssuedDate { get; set; }

		public string ExpirationDate { get; set; }

		public int DepartmentID { get; set; }

		public int PositionID { get; set; }

		public int CompanyID { get; set; }

		public byte[] Image { get; set; }

		public int CableTurn { get; set; }

		public string IdentityNo { get; set; }

		public DateTime IdentityDate { get; set; }

		public string IdentityPlace { get; set; }

		public int ZoneID { get; set; }

		public int MaxdayZone { get; set; }

		public string Description { get; set; }

		public string LicensePlate { get; set; }

		public int EmployeeTypeID { get; set; }

		public int EmployeeGroupID { get; set; }

		public int SiteID { get; set; }

		public int OriginID { get; set; }

		public int Status { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }

		public string EmployeeID { get; set; }

		public string MasterCode { get; set; }

		public string EmployeeCode { get; set; }

		public string EmployeeName { get; set; }

		public string DateOfBirth { get; set; }

		public string Sex { get; set; }

		public string WorkingDate { get; set; }

		public string Address { get; set; }

		public string Telephone { get; set; }

		public string PositionName { get; set; }

		public string DepartmentName { get; set; }
	}
}
