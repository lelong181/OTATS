using System;

namespace Ticket
{
	public class UsersModel : BaseModel
	{
		private int iD;

		private string loginName;

		private string passwordHash;

		private string saltKey;

		private string firstName;

		private string middleName;

		private string lastName;

		private int sex;

		private string jobDescription;

		private DateTime birthOfDate;

		private string telephone;

		private string handPhone;

		private string homeAddress;

		private string resident;

		private string postalCode;

		private int departmentID;

		private int status;

		private string communication;

		private DateTime passExpireDate;

		private bool isCashier;

		private int cashierNo;

		private string email;

		private DateTime startWorking;

		private string createdBy;

		private DateTime createdDate;

		private string updatedBy;

		private DateTime updatedDate;

		private int userGroupID;

		public int ID
		{
			get
			{
				return iD;
			}
			set
			{
				iD = value;
			}
		}

		public string LoginName
		{
			get
			{
				return loginName;
			}
			set
			{
				loginName = value;
			}
		}

		public string PasswordHash
		{
			get
			{
				return passwordHash;
			}
			set
			{
				passwordHash = value;
			}
		}

		public string SaltKey
		{
			get
			{
				return saltKey;
			}
			set
			{
				saltKey = value;
			}
		}

		public string FirstName
		{
			get
			{
				return firstName;
			}
			set
			{
				firstName = value;
			}
		}

		public string MiddleName
		{
			get
			{
				return middleName;
			}
			set
			{
				middleName = value;
			}
		}

		public string LastName
		{
			get
			{
				return lastName;
			}
			set
			{
				lastName = value;
			}
		}

		public int Sex
		{
			get
			{
				return sex;
			}
			set
			{
				sex = value;
			}
		}

		public string JobDescription
		{
			get
			{
				return jobDescription;
			}
			set
			{
				jobDescription = value;
			}
		}

		public DateTime BirthOfDate
		{
			get
			{
				return birthOfDate;
			}
			set
			{
				birthOfDate = value;
			}
		}

		public string Telephone
		{
			get
			{
				return telephone;
			}
			set
			{
				telephone = value;
			}
		}

		public int DepartmentID
		{
			get
			{
				return departmentID;
			}
			set
			{
				departmentID = value;
			}
		}

		public int Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
			}
		}

		public bool IsCashier
		{
			get
			{
				return isCashier;
			}
			set
			{
				isCashier = value;
			}
		}

		public int CashierNo
		{
			get
			{
				return cashierNo;
			}
			set
			{
				cashierNo = value;
			}
		}

		public string Email
		{
			get
			{
				return email;
			}
			set
			{
				email = value;
			}
		}

		public string CreatedBy
		{
			get
			{
				return createdBy;
			}
			set
			{
				createdBy = value;
			}
		}

		public DateTime CreatedDate
		{
			get
			{
				return createdDate;
			}
			set
			{
				createdDate = value;
			}
		}

		public string UpdatedBy
		{
			get
			{
				return updatedBy;
			}
			set
			{
				updatedBy = value;
			}
		}

		public DateTime UpdatedDate
		{
			get
			{
				return updatedDate;
			}
			set
			{
				updatedDate = value;
			}
		}

		public int UserGroupID
		{
			get
			{
				return userGroupID;
			}
			set
			{
				userGroupID = value;
			}
		}
	}
}
