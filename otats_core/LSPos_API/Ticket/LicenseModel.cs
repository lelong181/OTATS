namespace Ticket
{
	public class LicenseModel : BaseModel
	{
		private int iD;

		private int startingDate;

		private string companyName;

		private string address;

		private string tel;

		private string fax;

		private string email;

		private string account;

		private string district;

		private string city;

		private string branchName;

		private string taxCode;

		private string website;

		private decimal vIPAmount;

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

		public int StartingDate
		{
			get
			{
				return startingDate;
			}
			set
			{
				startingDate = value;
			}
		}

		public string CompanyName
		{
			get
			{
				return companyName;
			}
			set
			{
				companyName = value;
			}
		}

		public string Address
		{
			get
			{
				return address;
			}
			set
			{
				address = value;
			}
		}

		public string Tel
		{
			get
			{
				return tel;
			}
			set
			{
				tel = value;
			}
		}

		public string Fax
		{
			get
			{
				return fax;
			}
			set
			{
				fax = value;
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

		public string Account
		{
			get
			{
				return account;
			}
			set
			{
				account = value;
			}
		}

		public string District
		{
			get
			{
				return district;
			}
			set
			{
				district = value;
			}
		}

		public string City
		{
			get
			{
				return city;
			}
			set
			{
				city = value;
			}
		}

		public string BranchName
		{
			get
			{
				return branchName;
			}
			set
			{
				branchName = value;
			}
		}

		public string TaxCode
		{
			get
			{
				return taxCode;
			}
			set
			{
				taxCode = value;
			}
		}

		public string Website
		{
			get
			{
				return website;
			}
			set
			{
				website = value;
			}
		}

		public decimal VIPAmount
		{
			get
			{
				return vIPAmount;
			}
			set
			{
				vIPAmount = value;
			}
		}
	}
}
