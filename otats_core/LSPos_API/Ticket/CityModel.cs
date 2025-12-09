using System;

namespace Ticket
{
	public class CityModel : BaseModel
	{
		private int iD;

		private string name;

		private bool inactive;

		private DateTime createdDate;

		private string createdBy;

		private DateTime updatedDate;

		private string updatedBy;

		public string Description { get; set; }

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

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
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
	}
}
