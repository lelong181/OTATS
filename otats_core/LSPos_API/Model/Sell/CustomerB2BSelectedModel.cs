using System;

namespace BusinessLayer.Model.Sell
{
	public class CustomerB2BSelectedModel
	{
		public Guid CustomerID { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public string IdOrPPNum { get; set; }

		public string PhoneNumber { get; set; }

		public string Address { get; set; }

		public string CustomerType { get; set; }

		public string CustomerTypeStr { get; set; }

		public byte[] Avatar { get; set; }

		public Guid ID
		{
			get
			{
				return CustomerID;
			}
			set
			{
				value = CustomerID;
			}
		}
	}
}
