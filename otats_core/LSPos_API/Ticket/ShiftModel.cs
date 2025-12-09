using System;

namespace Ticket
{
	public class ShiftModel : BaseModel
	{
		private int iD;

		private DateTime loginTime;

		private DateTime logoutTime;

		private new int userID;

		private string userName;

		private bool status;

		private int userInsertID;

		private int userUpdateID;

		private DateTime createDate;

		private DateTime updateDate;

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

		public DateTime LoginTime
		{
			get
			{
				return loginTime;
			}
			set
			{
				loginTime = value;
			}
		}

		public DateTime LogoutTime
		{
			get
			{
				return logoutTime;
			}
			set
			{
				logoutTime = value;
			}
		}

		public int UserID
		{
			get
			{
				return userID;
			}
			set
			{
				userID = value;
			}
		}

		public string UserName
		{
			get
			{
				return userName;
			}
			set
			{
				userName = value;
			}
		}

		public bool Status
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

		public int UserInsertID
		{
			get
			{
				return userInsertID;
			}
			set
			{
				userInsertID = value;
			}
		}

		public int UserUpdateID
		{
			get
			{
				return userUpdateID;
			}
			set
			{
				userUpdateID = value;
			}
		}

		public DateTime CreateDate
		{
			get
			{
				return createDate;
			}
			set
			{
				createDate = value;
			}
		}

		public DateTime UpdateDate
		{
			get
			{
				return updateDate;
			}
			set
			{
				updateDate = value;
			}
		}
	}
}
