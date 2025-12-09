using System;

namespace Ticket
{
	public class AccountDetailModel : BaseModel
	{
		private int iD;

		private int accountID;

		private int accountTypeID;

		private DateTime issuedDate;

		private DateTime expirationDate;

		private double totalMoney;

		private int status;

		private DateTime createdDate;

		private string createdBy;

		private DateTime updatedDate;

		private string updatedBy;

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

		public int AccountID
		{
			get
			{
				return accountID;
			}
			set
			{
				accountID = value;
			}
		}

		public int AccountTypeID
		{
			get
			{
				return accountTypeID;
			}
			set
			{
				accountTypeID = value;
			}
		}

		public DateTime IssuedDate
		{
			get
			{
				return issuedDate;
			}
			set
			{
				issuedDate = value;
			}
		}

		public DateTime ExpirationDate
		{
			get
			{
				return expirationDate;
			}
			set
			{
				expirationDate = value;
			}
		}

		public double TotalMoney
		{
			get
			{
				return totalMoney;
			}
			set
			{
				totalMoney = value;
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
	}
}
