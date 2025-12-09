using System;

namespace BusinessLayer.Model.API
{
	public class RptWalletRes
	{
		public string Status { get; set; }

		public string AccountCode { get; set; }

		public DateTime? TransactionDate { get; set; }

		public DateTime? IssuedDate { get; set; }

		public DateTime? ExpirationDate { get; set; }

		public decimal CreditLimit { get; set; }

		public decimal TotalMoney { get; set; }

		public decimal Deposit { get; set; }

		public decimal Balance { get; set; }

		public string Description { get; set; }

		public string Fullname { get; set; }

		public string CellPhone { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public DateTime? BirthDay { get; set; }
	}
}
