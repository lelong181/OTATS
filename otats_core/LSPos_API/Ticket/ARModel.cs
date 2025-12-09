using System;

namespace Ticket
{
	public class ARModel
	{
		public Guid ID { get; set; }

		public string AccountNo { get; set; }

		public Guid ProfileID { get; set; }

		public decimal CreditLimit { get; set; }

		public int CreditDueDays { get; set; }

		public decimal Balance { get; set; }

		public decimal BalanceFuture { get; set; }

		public int Type { get; set; }

		public string Description { get; set; }

		public Guid AccountReceivableTypeID { get; set; }

		public bool IsCentralized { get; set; }

		public string Email { get; set; }

		public string ProfileCode { get; set; }

		public string Name { get; set; }

		public string IdentityCard { get; set; }

		public DateTime? BirthDate { get; set; }

		public string AlternativeEmail { get; set; }

		public string CellPhone { get; set; }

		public string PhoneNumber { get; set; }

		public string TypeStr { get; set; }

		public decimal SoDu => CreditLimit - Balance;
	}
}
