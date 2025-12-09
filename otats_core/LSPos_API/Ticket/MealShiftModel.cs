namespace Ticket
{
	public class MealShiftModel : BaseModel
	{
		private int iD;

		private string code;

		private string name;

		private bool inactive;

		private int type;

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

		public string Code
		{
			get
			{
				return code;
			}
			set
			{
				code = value;
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

		public int Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}
	}
}
