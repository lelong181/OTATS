using System;

namespace BusinessLayer.Model.Sell
{
	public class CustomerSelectedModel
	{
		public Guid CustomerID { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public string Address { get; set; }

		public string Tax { get; set; }

		public string BankAccount { get; set; }
	}
}
