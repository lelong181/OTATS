using System;

namespace Ticket
{
	public class AccountModel : BaseModel
	{
		public int ID { get; set; }

		public string AccountCode { get; set; }

		public string CardID { get; set; }

		public int ServiceTypeID { get; set; }

		public int ServiceID { get; set; }

		public DateTime IssuedDate { get; set; }

		public DateTime ExpirationDate { get; set; }

		public double TotalMoney { get; set; }

		public int Status { get; set; }

		public int EmployeeID { get; set; }

		public int ProfileID { get; set; }

		public int BookingID { get; set; }
	}
}
