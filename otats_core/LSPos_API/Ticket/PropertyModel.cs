using System;

namespace Ticket
{
	public class PropertyModel : BaseModel
	{
		private int iD;

		private int propertyTypeID;

		private string propertyCode;

		private string propertyName;

		private string telephone;

		private string fax;

		private string email;

		private string website;

		private string address;

		private string serverName;

		private string databaseName;

		private string login;

		private string password;

		private bool inactive;

		private string createdBy;

		private DateTime createdDate;

		private string updatedBy;

		private DateTime updatedDate;

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

		public int PropertyTypeID
		{
			get
			{
				return propertyTypeID;
			}
			set
			{
				propertyTypeID = value;
			}
		}

		public string PropertyCode
		{
			get
			{
				return propertyCode;
			}
			set
			{
				propertyCode = value;
			}
		}

		public string PropertyName
		{
			get
			{
				return propertyName;
			}
			set
			{
				propertyName = value;
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

		public string ServerName
		{
			get
			{
				return serverName;
			}
			set
			{
				serverName = value;
			}
		}

		public string DatabaseName
		{
			get
			{
				return databaseName;
			}
			set
			{
				databaseName = value;
			}
		}

		public string Login
		{
			get
			{
				return login;
			}
			set
			{
				login = value;
			}
		}

		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				password = value;
			}
		}

		public bool Inactive
		{
			get
			{
				return inactive;
			}
			set
			{
				inactive = value;
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
	}
}
