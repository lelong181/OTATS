using System;

namespace BusinessLayer.Model.API
{
	public class BookingCustomerResponse
	{
		public Guid ID { get; set; }

		public string Address { get; set; }

		public string CountryInPP { get; set; }

		public string Email { get; set; }

		public string IdOrPPNum { get; set; }

		public string ImageType { get; set; }

		public byte[] Avatar { get; set; }

		public string Name { get; set; }

		public string PhoneNumber { get; set; }

		public string Note { get; set; }

		public string CustomerType { get; set; }
	}
}
