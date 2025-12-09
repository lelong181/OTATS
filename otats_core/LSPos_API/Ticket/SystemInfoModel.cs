using System;

namespace Ticket
{
	public class SystemInfoModel : BaseModel
	{
		private int iD;

		private string keyName;

		private string keyValue;

		private string description;

		private int status;

		private DateTime createdDate;

		private string createdBy;

		private DateTime updatedDate;

		private string updatedBy;

		private bool isAllowEdit;

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

		public string KeyName
		{
			get
			{
				return keyName;
			}
			set
			{
				keyName = value;
			}
		}

		public string KeyValue
		{
			get
			{
				return keyValue;
			}
			set
			{
				keyValue = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
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

		public bool IsAllowEdit
		{
			get
			{
				return isAllowEdit;
			}
			set
			{
				isAllowEdit = value;
			}
		}
	}
}
